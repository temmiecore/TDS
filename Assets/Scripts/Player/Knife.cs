using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    private Animator anim;
    public AudioManager audio;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Mouse1) && !GameManager.instance.isKnife)
        {
            GameManager.instance.knife.gameObject.SetActive(false);
            GameManager.instance.bow.gameObject.SetActive(true);
            GameManager.instance.isKnife = true;
            GameManager.instance.player.weaponcam.transform.localScale = Vector3.one;
        }
    }

    protected override void Attack()
    {
        audio.Play("Sword2");
        anim.SetTrigger("SwingTr");
    }
}
