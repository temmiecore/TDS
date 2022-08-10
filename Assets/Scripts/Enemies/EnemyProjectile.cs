using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Collidable
{
    public float push = 0.5f;
    public float liveT = 1f;
    public GameObject aftereffect;
    public int damage;

    protected override void Start()
    {
        base.Start();
    }


    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            Damage dmg = new Damage()
            {
                damageRecieved = damage,
                origin = transform.position,
                pushForce = push,
            };
            coll.SendMessage("ReceiveDamage", dmg);
            Destroy(gameObject);
        }
        if (!(coll.tag == "Mob" || coll.tag == "Hitbox" || coll.tag == "Arrow" ||coll.tag == "Pick-Ups" || coll.tag == "Boss"))
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
