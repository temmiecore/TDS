using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : State
{

    public GameObject projectile;
    public Transform circlingPoint;

    Transform playerTr;
    BoxCollider2D boxCollider;
    Vector3 moveDistance;
    Rigidbody2D rb;
    SpriteRenderer moverSprite;

    private void Start()
    {
        playerTr = GameManager.instance.player.transform;
        boxCollider = GetComponentInParent<BoxCollider2D>();
        rb = GetComponentInParent<Rigidbody2D>();
        moverSprite = GetComponentInParent<SpriteRenderer>();
    }

    private void UpdateMotor(Vector3 input)
    {
        moveDistance = input;
        if (moveDistance.x < 0)
            moverSprite.flipX = true;
        else if (moveDistance.x > 0)
            moverSprite.flipX = false;

        //Hit detection
        RaycastHit2D hit = Physics2D.BoxCast(transform.parent.position, boxCollider.size, 0, new Vector2(0, moveDistance.y), Mathf.Abs(moveDistance.y * Time.deltaTime), LayerMask.GetMask("Actors", "Blocking")); //For y axis
        if (hit.collider == null)
        { transform.parent.Translate(0, moveDistance.y * Time.deltaTime, 0); }

        hit = Physics2D.BoxCast(transform.parent.position, boxCollider.size, 0, new Vector2(moveDistance.x, 0), Mathf.Abs(moveDistance.x * Time.deltaTime), LayerMask.GetMask("Actors", "Blocking")); // x
        if (hit.collider == null)
        { transform.parent.Translate(moveDistance.x * Time.deltaTime, 0, 0); }
    }

    private void Shoot(Vector3 direction, float speed)
    {
        GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
        Rigidbody2D rbproj = arrow.GetComponent<Rigidbody2D>();

        rbproj.transform.right = direction;
        rbproj.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
    }

  

    IEnumerator Shooting()
    {
        Debug.Log("Shooting!");
        //*Blink* sound
        stopTrigger = true;
        ////Idle anim
        yield return new WaitForSeconds(0.3f);
        stopTrigger = false;
        ////Walk anim
        isCircling = true;
        Shoot(direction, 1.6f);
        //Fireball sound
        yield return new WaitForSeconds(1f);
        Shoot(direction, 1.4f);
        //Fireball sound
        yield return new WaitForSeconds(1f);
        Shoot(direction, 1.4f);
        //Fireball sound
        yield return new WaitForSeconds(0.3f);
        Shoot(direction, 1.4f);
        //Fireball sound
        yield return new WaitForSeconds(0.3f);
        Shoot(direction, 1.4f);
        //Fireball sound
        stopTrigger = true;
        ////Idle anim
        isCircling = false;
        yield return new WaitForSeconds(0.3f);
        ////Walk anim
        stopTrigger = false;
    }

    IEnumerator Dashing()
    {
        Debug.Log("Dashing!");
        //*Blink* sound
        stopTrigger = true;
        ////Idle anim
        yield return new WaitForSeconds(0.8f);
        ////Dash Anim trigger? Or just Walk anim
        rb.AddForce(direction.normalized * 1.7f, ForceMode2D.Impulse);
        //Woosh sound
        yield return new WaitForSeconds(0.6f);
        rb.AddForce(direction.normalized * 1.7f, ForceMode2D.Impulse);
        //Woosh sound
        yield return new WaitForSeconds(0.6f);
        rb.AddForce(direction.normalized * 2f, ForceMode2D.Impulse);
        //Woosh sound
        ////Idle anim
        yield return new WaitForSeconds(0.3f);
        ////Walk anim
        stopTrigger = false;
    }


    Vector3 direction;
    float spawndelay;
    bool stopTrigger = false;
    bool isCircling = false;
    int phaze = 1;
    public override State RunCurrentState()
    {
        direction = playerTr.position - transform.parent.position;

        if (Time.realtimeSinceStartup - spawndelay > 9f && phaze == 1)
        { StartCoroutine(Shooting()); phaze = 2; spawndelay = Time.realtimeSinceStartup; }
        else if (Time.realtimeSinceStartup - spawndelay > 9f && phaze == 2)
        { StartCoroutine(Dashing()); phaze = 1; spawndelay = Time.realtimeSinceStartup; }


        if (!stopTrigger && !isCircling)
            UpdateMotor((direction * 10f).normalized * 0.4f);
        else if (!stopTrigger && isCircling)
            UpdateMotor(new Vector3((circlingPoint.position - transform.parent.position).normalized.x * 0.4f, (circlingPoint.position - transform.parent.position).normalized.y * 0.4f, 0));

        return this;
    }
}
