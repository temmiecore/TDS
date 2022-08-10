using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    float bowCooldown = 0.5f, lastbow;

    public GameObject arrowpf;
    public float arrowforce = 2f;
    public Transform firepos;
    public AudioManager audio;


    protected override void Update()
    {
        Vector3 mousepos = Input.mousePosition;
        mousepos = Camera.main.ScreenToWorldPoint(mousepos);
        base.FixedUpdate();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Time.time - lastbow > bowCooldown)
            {
                lastbow = Time.time;
                Attack(mousepos);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && GameManager.instance.isKnife)
        {
            GameManager.instance.knife.gameObject.SetActive(true);
            GameManager.instance.bow.gameObject.SetActive(false);
            GameManager.instance.isKnife = false;
        }
    }

    private void Attack(Vector3 mousepos)
    {
        audio.Play("Shoot3");
        GameObject arrow = Instantiate(arrowpf, firepos.position, firepos.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        rb.AddForce(firepos.up * arrowforce, ForceMode2D.Impulse);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        spriteRenderer.sprite = GameManager.instance.bowSprites[GameManager.instance.weaponLevel];
    }
}
