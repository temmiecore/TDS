using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : State
{
    Transform playerTr;
    BoxCollider2D boxCollider;
    Vector3 moveDistance;
    SpriteRenderer moverSprite;
    Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponentInParent<Rigidbody2D>();
        playerTr = GameManager.instance.player.transform;
        boxCollider = GetComponentInParent<BoxCollider2D>();
        moverSprite = GetComponentInParent<SpriteRenderer>();
    }

    private float idledelay;
    private void UpdateMotor(Vector3 input)
    {
        moveDistance = input;
        if (moveDistance.x < 0)
            moverSprite.flipX = true;
        else if (moveDistance.x > 0)
            moverSprite.flipX = false;

        transform.parent.Translate(moveDistance.x * Time.deltaTime, moveDistance.y * Time.deltaTime, 0); 
    }

    bool stopTrigger = false;
    Vector3 direction;

    IEnumerator Dash()
    { 
        stopTrigger = true;
        yield return new WaitForSeconds(0.2f);
        /*Blink sound*/
        rigid.AddForce(direction.normalized * 5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.4f);
        stopTrigger = false; 
    }

    public override State RunCurrentState()
    {
        direction = playerTr.position - transform.parent.position;

        if (Time.realtimeSinceStartup - idledelay > Random.Range(4f, 7f))
        {
            StartCoroutine(Dash());
            idledelay = Time.realtimeSinceStartup;
        }

        if (!stopTrigger)
            UpdateMotor(direction.normalized * 0.35f);
        return this;
    }

}
