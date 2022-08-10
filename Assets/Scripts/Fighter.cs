using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int hitpoint = 10, maxHitpoint = 10;
    public float pushrecovery;

    public float immunity = 0.5f, lastImmune;
    protected Vector3 pushDirection;

    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immunity)
        {
            Debug.Log("Push recovery is " + pushrecovery);
            lastImmune = Time.time;
            hitpoint -= dmg.damageRecieved;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            FindObjectOfType<AudioManager>().Play("Enemy_Damage");
            GameManager.instance.Showtext("-" + dmg.damageRecieved, Color.red, transform.position,1);
            if (hitpoint <= 0)
            {
                hitpoint = 0; Death();
            }
        }
    }
    protected virtual void Death()
    {
        
    }
}
