using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Fighter
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        anim.SetTrigger("DummyHit");
    }

    protected override void Death()
    {
        Destroy(gameObject);
    }
}
