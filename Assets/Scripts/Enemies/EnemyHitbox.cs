using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    public int dmgpoint = 1;
    public float pushforce;

    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);
        if (coll.name == "Player")
        {
            Damage damage = new Damage
            {
                damageRecieved = dmgpoint,
                origin = transform.position,
                pushForce = pushforce
            };

            coll.SendMessage("ReceiveDamage", damage);
        }
    }
}
