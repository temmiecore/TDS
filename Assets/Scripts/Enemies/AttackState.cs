using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public float dashCoolDown;
    public ChaseState chaseState;

    Transform playerTr;
    Rigidbody2D rb;
    SpriteRenderer moverSprite;

    private void Start()
    {
        playerTr = GameManager.instance.player.transform;
        rb = GetComponentInParent<Rigidbody2D>();
        moverSprite = GetComponentInParent<SpriteRenderer>();
    }

    float idledelay;
    Vector3 direction;
    public override State RunCurrentState()
    {
        direction = playerTr.position - transform.parent.position;

        if (direction.x < 0)
            moverSprite.flipX = true;
        else if (direction.x > 0)
            moverSprite.flipX = false;

        if (Time.realtimeSinceStartup - idledelay > dashCoolDown)
        { 
            rb.AddForce((direction * 10f).normalized * 1.6f, ForceMode2D.Impulse); 
            //Woosh sound
            idledelay = Time.realtimeSinceStartup; 
        }

        if (Vector3.Distance(playerTr.position, transform.parent.position) > 3.5 * 0.16f)
            return chaseState;
        else
            return this;
    }
}
