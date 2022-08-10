using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    public int[] damage = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    public float[] push = { 1f, 1f,1f, 1.5f, 1.5f, 1.5f, 2f, 2f, 2f, 3f };

    public SpriteRenderer spriteRenderer;
    public bool isInMenu;
    private float atkCooldown = 0.5f, lastatk;


    protected override void Start()
    {
        base.Start();
        isInMenu = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void Update()
    {
        if (isInMenu == false)
        {
            base.FixedUpdate();
            if (Input.GetKeyDown(KeyCode.Mouse0))
                if (Time.time - lastatk > atkCooldown)
                {
                    lastatk = Time.time;
                    Attack();
                }
        }
    }
    protected virtual void Attack()
    {
        Debug.Log("Attack!!");
    }
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Mob" || coll.tag == "Boss" || coll.tag == "Prop")
        {
            Damage dmg = new Damage()
            {
                damageRecieved = damage[GameManager.instance.weaponLevel],
                origin = transform.position,
                pushForce = push[GameManager.instance.weaponLevel],
            };
            coll.SendMessage("ReceiveDamage", dmg);
        }
    }
    public virtual void Upgrade()
    {
        GameManager.instance.weaponLevel++;
    }
}
