using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : State
{
    public Boss1 boss1State;
    public Boss2 boss2State;
    public Boss3 boss3State;
    public int bossType;
    public float idleSpeed;

    Transform playerTr;
    SpriteRenderer moverSprite;


    private void Start()
    {
        playerTr = GameManager.instance.player.transform;
        moverSprite = GetComponentInParent<SpriteRenderer>();
    }

    float idledelay;
    bool stopTrigger = false;
    float randX, randY;

    IEnumerator Wait()
    {
        stopTrigger = true;
        randX = Random.Range(-1f, 1f);
        randY = Random.Range(-1f, 1f);
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        stopTrigger = false;
    }

    public override State RunCurrentState()
    {

        if (Time.realtimeSinceStartup - idledelay > Random.Range(4f, 7f))
        { StartCoroutine(Wait()); idledelay = Time.realtimeSinceStartup; }

        if (stopTrigger)
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, new Vector2(transform.parent.position.x + randX, transform.parent.position.y + randY), idleSpeed * Time.deltaTime);

        if (randX < 0)
            moverSprite.flipX = true;
        else if (randX > 0)
            moverSprite.flipX = false;

        Vector3 direction = playerTr.position - transform.parent.position;
        RaycastHit2D seePlayer = Physics2D.Raycast(transform.parent.position, direction);

        if (seePlayer.collider.name == "Player" && Vector3.Distance(playerTr.position, transform.parent.position) < 10 * 0.16f)
        {
            switch (bossType)
            {
                case 0:
                    {
                        GameManager.instance.bossHP.Initialize(70);
                        return boss1State;
                    }
                case 1:
                    {
                        GameManager.instance.bossHP.Initialize(120);
                        return boss2State;
                    }
                case 2:
                    {
                        GameManager.instance.bossHP.Initialize(170);
                        return boss3State;
                    }
            }
        }

        return this;
    }
}
