using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private Animator anim;
    public AudioManager audio;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }
    protected override void Attack()
    {
        audio.Play("Sword2");
        anim.SetTrigger("SwingTr");
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void Upgrade()
    {
        base.Upgrade();
        spriteRenderer.sprite = GameManager.instance.swordSprites[GameManager.instance.weaponLevel];
    }
}
