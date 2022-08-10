using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    private Animator animw;
    public GameObject projpf;
    public float projforce = 5f;
    public Transform firepos;
    private float staffCooldown = 0.5f, laststaff;
    public AudioManager audio;
    public int manaCost = 2;

    protected override void Start()
    {
        base.Start();
        animw = GetComponent<Animator>();
    }
    protected override void Update()
    {
        Vector3 mousepos = Input.mousePosition;
        mousepos = Camera.main.ScreenToWorldPoint(mousepos);
        base.FixedUpdate();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Time.time - laststaff > staffCooldown)
            {
                if (GameManager.instance.player.mana >= manaCost)
                {

                    laststaff = Time.time;
                    GameManager.instance.player.mana -= manaCost;
                    GameManager.instance.ManaChange();
                    Attack(mousepos);

                }
                else
                {
                    laststaff = Time.time;
                    GameManager.instance.Showtext("Not enough mana!", Color.blue, GameManager.instance.player.transform.position,1);
                }
            }
        }
    }
    private void Attack(Vector3 mousepos)
    {
        audio.Play("Magic2");
        GameObject proj = Instantiate(projpf, firepos.position, Quaternion.Euler(firepos.rotation.eulerAngles.x, firepos.rotation.eulerAngles.y, firepos.rotation.eulerAngles.z - 90f));
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.AddForce(firepos.right * projforce, ForceMode2D.Impulse);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        GameManager.instance.player.SetSprite(GameManager.instance.sageAnims[GameManager.instance.weaponLevel - 1]);
    }
}
