using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public AttackRangeState attackRangeState;
    public IdleState idleState;
    public PatrolState patrolState;
    public float speed = 0.4f;
    public bool isRange;
    public bool isPatrol;

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
    float distance;
    public override State RunCurrentState()
    {
        direction = playerTr.position - transform.parent.position;
        distance = Vector3.Distance(playerTr.position, transform.parent.position);

        if (distance < 3.5 * 0.16f && !isRange)
            return attackState;
        else if (distance < 5 * 0.16f && isRange)
            return attackRangeState;

        if (distance > 9 * 0.16f && !isPatrol)
            return idleState;
        if (distance > 9 * 0.16f && isPatrol)
            return patrolState;

        UpdateMotor(direction * 0.4f);

        return this;
    }
}
