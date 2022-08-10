using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public Transform[] points;
    public float triggerLength = 1f;
    public ChaseState chaseState;
    public float speed;

    Transform playerTr;
    SpriteRenderer moverSprite;

    private void Start()
    {
        playerTr = GameManager.instance.player.transform;
        currentpoint = 0;
        moverSprite = GetComponentInParent<SpriteRenderer>();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        if (currentpoint + 1 < points.Length)
            currentpoint++;
        else
            currentpoint = 0;
        once = false;
    }

    int currentpoint;
    bool once;
    Vector3 direction;
    RaycastHit2D seePlayer;
    public override State RunCurrentState()
    {
        if (transform.parent.position != points[currentpoint].position)
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, points[currentpoint].position, speed * Time.deltaTime);
        else
        { 
            if (!once) 
            { once = true; StartCoroutine(Wait()); }
        }

        if ((points[currentpoint].position - transform.parent.position).x < 0)
            moverSprite.flipX = true;
        else if ((points[currentpoint].position - transform.parent.position).x > 0)
            moverSprite.flipX = false;

        direction = playerTr.position - transform.parent.position;
        seePlayer = Physics2D.Raycast(transform.parent.position, direction);

        if (seePlayer.collider.name == "Player" && Vector3.Distance(playerTr.position, transform.parent.position) < 9 * 0.16f)
            return chaseState;
        return this;
    }
}
