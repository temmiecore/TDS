using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : State
{
    public Transform[] projectiles;
    public float[] projectileSpeed;
    public GameObject boyPrefab;

    Transform playerTr;
    BoxCollider2D boxCollider;
    Vector3 moveDistance;
    SpriteRenderer moverSprite;

    private void Start()
    {
        playerTr = GameManager.instance.player.transform;
        boxCollider = GetComponentInParent<BoxCollider2D>();
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


    Vector3 direction;
    float spawndelay;
    bool stopTrigger = false;
    IEnumerator SpawnBoys()
    {
        stopTrigger = true;
        //*Blink* sound
        yield return new WaitForSeconds(1f);
        //Growing plants sound
        Instantiate(boyPrefab, new Vector3(transform.parent.position.x + 0.2f, transform.parent.position.y + 0.2f, 0), Quaternion.identity);
        Instantiate(boyPrefab, new Vector3(transform.parent.position.x - 0.2f, transform.parent.position.y - 0.2f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        stopTrigger = false;
    }

    public override State RunCurrentState()
    {
        direction = playerTr.position - transform.parent.position;
        for (int i = 0; i < projectiles.Length; i++)
            projectiles[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time * projectileSpeed[i]) * 0.16f, Mathf.Sin(Time.time * projectileSpeed[i]) * 0.16f, 0);

        if (Time.realtimeSinceStartup - spawndelay > 15f)
        {
            StartCoroutine(SpawnBoys());
            spawndelay = Time.realtimeSinceStartup;
        }

        if (!stopTrigger)
            UpdateMotor((direction * 10f).normalized * 0.3f);
        
        return this;
    }
}
