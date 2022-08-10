using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Collidable
{
    public int healAmount = 1;

    private float healCD = 1f;
    private float lastHeal;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
            if (Time.time - lastHeal > healCD)
            {
                lastHeal = Time.time;
                GameManager.instance.player.HealPlayer(healAmount);
            }
    }
}
