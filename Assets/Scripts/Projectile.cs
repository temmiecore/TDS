using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Collidable
{
    public float push = 0.5f;
    public float liveT = 1f;
    public GameObject aftereffect;

    private int damage;
    protected override void Start()
    {
        base.Start();
    }


    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Mob" || coll.tag == "Prop" || coll.tag == "Boss")
        {
            if (GameManager.instance.playerClass == 1)
                damage = GameManager.instance.bow.damage[GameManager.instance.weaponLevel];
            else if (GameManager.instance.playerClass == 2)
                damage = GameManager.instance.staff.damage[GameManager.instance.weaponLevel];

            Damage dmg = new Damage()
            {
                damageRecieved = damage,
                origin = transform.position,
                pushForce = push,
            };
            coll.SendMessage("ReceiveDamage", dmg);
            Destroy(gameObject);
        }
        if (!(coll.tag == "Player" || coll.tag == "Hitbox" || coll.tag == "Pick-Ups"))
        {
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, liveT);
            try
            {
                GameObject effect = Instantiate(aftereffect, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(effect, 0.6f);
            }
            catch (UnassignedReferenceException e)
            { 
                
            }
        }

    }

}
