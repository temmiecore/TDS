using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : Movers
{
    //STATE CONTROL SCRIPT
    public State currState;
    public int xpValue = 1;

    protected override void Start()
    {
        base.Start();
    }

    private void RunStateMachine()
    {
        State nextState = currState?.RunCurrentState();
        if (nextState != null)
            ChangeState(nextState);
    }

    private void ChangeState(State state)
    { currState = state; }

    public void Update()
    { RunStateMachine(); }

    
    protected override void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immunity)
        {
            Debug.Log("Push recovery is " + pushrecovery);
            lastImmune = Time.time;
            hitpoint -= dmg.damageRecieved;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
            if (tag == "Boss")
                GameManager.instance.bossHP.UpdateContainer(dmg.damageRecieved);
            FindObjectOfType<AudioManager>().Play("Enemy_Damage");
            GameManager.instance.Showtext("-" + dmg.damageRecieved, Color.red, transform.position,1);
            if (hitpoint <= 0)
            {
                hitpoint = 0; Death();
            }
        }
    }

    public List<GameObject> prefabsList; //IF playerclass = bow, drop arrows, if mage - drop manapots
    protected override void Death()
    {
        FindObjectOfType<AudioManager>().Play("Enemy_Dies");
        Destroy(gameObject);
        GameManager.instance.GainXp(xpValue);
        GameManager.instance.Showtext("+" + xpValue + " XP", Color.white,
                new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 0.25f, 0),1);
        GameManager.instance.DropItems(2, prefabsList, gameObject);

        if (tag == "Boss")
        { 
            GameManager.instance.isBossDead = true;
            GameManager.instance.bossHP.Died();
        }
    }
}
