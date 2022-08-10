using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeState : State
{
    public GameObject projectile;
    public Transform circlingPoint;
    public ChaseState chaseState;

    Transform playerTr;
    BoxCollider2D boxCollider;
    Vector3 moveDistance;
    SpriteRenderer moverSprite;

    private void Start()
    {
        playerTr = GameManager.instance.player.transform;
        transform.parent = GetComponentInParent<Transform>();
        boxCollider = GetComponentInParent<BoxCollider2D>();
        moverSprite = GetComponentInParent<SpriteRenderer>();
    }

    private void UpdateMotor(Vector3 input)
    {
        moveDistance = input;

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
        //Тетива sound
        rbproj.transform.up = direction;
        rbproj.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
    }

    float idledelay;
    float changedelay;
    float randomness;
    bool changeTactic;
    Vector3 direction;
    public override State RunCurrentState()
    {
        direction = playerTr.position - transform.parent.position;
        if (Time.realtimeSinceStartup - idledelay > 2.4f)
        {
            Shoot(direction, 0.75f);
            idledelay = Time.realtimeSinceStartup;
            randomness = Random.Range(-0.32f, 0.32f);
        }

        if (direction.x < 0)
            moverSprite.flipX = true;
        else if (direction.x > 0)
            moverSprite.flipX = false;

        if (Time.realtimeSinceStartup - changedelay > Random.Range(2f, 7f))
        { changeTactic = !changeTactic; changedelay = Time.realtimeSinceStartup; }

        if (Vector3.Distance(playerTr.position, transform.parent.position) < 2.5 * 0.16f)
            UpdateMotor((-direction).normalized * 0.14f);

        if (changeTactic)
            UpdateMotor(new Vector3(((circlingPoint.position - transform.parent.position).normalized.x + randomness) * 0.23f, ((circlingPoint.position - transform.parent.position).normalized.y + randomness) * 0.3f, 0));
        else
            UpdateMotor(-(new Vector3((circlingPoint.position - transform.parent.position).normalized.x * 0.23f, (circlingPoint.position - transform.parent.position).normalized.y * 0.3f, 0)));

        if (Vector3.Distance(playerTr.position, transform.parent.position) > 5 * 0.16f)
            return chaseState;

        return this;
    }
}
