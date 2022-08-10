using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movers
{
    public float speedX = 1f, speedY = 0.75f;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool isAlive;
    public int maxMana = 10;
    public int mana = 10;
    public float manaRecovery = 1f;
    public float lastmana;
    public Animator menu;
    private bool menuOpen;
    public bool isInMenu;
    private float delay;
    public GameObject menuobj;
    public int arrows = 10;
    InventoryMenu inventor;

    protected override void Start()
    {
        base.Start();
        isAlive = true;
        isInMenu = true;
        menuOpen = false;
        inventor = menuobj.GetComponent<InventoryMenu>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void HealPlayer(int hp)
    {
        if (hitpoint == maxHitpoint)
        {
            GameManager.instance.Showtext("Full Health!", Color.green, transform.position,1);
            return;
        }
        hitpoint += hp;
        if (hitpoint > maxHitpoint)
            hitpoint = maxHitpoint;

        GameManager.instance.Showtext("+" + hp + "HP!", Color.green, transform.position,1);
        GameManager.instance.HPchange();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (isAlive)
        {
            if (Time.time - lastImmune > immunity)
            {
                lastImmune = Time.time;
                hitpoint -= dmg.damageRecieved;
                pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

                FindObjectOfType<AudioManager>().Play("Hero_Hurt");
                anim.SetTrigger("OuchTr");
                GameManager.instance.Showtext("-" + dmg.damageRecieved, Color.red, transform.position, 1);
                if (hitpoint <= 0)
                {
                    hitpoint = 0; Death();
                }
            }
            GameManager.instance.HPchange();
        }
    }

    private void FixedUpdate()
    {
        if (isAlive && !isInMenu)
        {
            if (mana < maxMana && Time.time - lastmana > manaRecovery)
            {
                mana++;
                lastmana = Time.time;
                GameManager.instance.ManaChange();
            }

            if (Input.GetKey(KeyCode.Tab) && menuOpen == false && Time.realtimeSinceStartup - delay > 0.3) //i can't believe i have to do it like this
            {
                inventor.UpdateMenu();
                GameManager.instance.hUD.GetComponent<CanvasGroup>().alpha = 0f;
                menu.SetBool("hidden", false);
                delay = Time.realtimeSinceStartup;
                menuOpen = true;
            }

            if (Input.GetKey(KeyCode.Tab) && menuOpen == true && Time.realtimeSinceStartup - delay > 0.3)
            {
                menu.SetBool("hidden", true);
                GameManager.instance.hUD.GetComponent<CanvasGroup>().alpha = 1f;
                delay = Time.realtimeSinceStartup;
                menuOpen = false;
            }

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            if (x == 1 || x == -1 || y == 1 || y == -1)
                anim.SetBool("WillWalk", true);
            else
                anim.SetBool("WillWalk", false);

            UpdateMotor(new Vector3(x * speedX, y * speedY, 0));
        }

    }

    public void SetSprite(int spriteID, RuntimeAnimatorController animatorC)
    {
        spriteRenderer.sprite = GameManager.instance.playerClasses[spriteID];
        anim.runtimeAnimatorController = animatorC;
    }
    public void SetSprite(RuntimeAnimatorController animatorC)
    {
        anim.runtimeAnimatorController = animatorC;
    }

    public void OnLevelUp()
    {
        maxHitpoint += 2;
        maxMana += 1;
        mana = maxMana;
        hitpoint = maxHitpoint;
        Debug.Log("Level up!.");
    }

    public void SetLevel(int savedlvl)
    {
        maxHitpoint = 5;
        maxMana = 10;
        for (int i = 1; i < savedlvl; i++)
        {
            OnLevelUp();
        }
    }

    protected override void Death()
    {
        FindObjectOfType<AudioManager>().Play("Hero_Death");
        GameManager.instance.deathMenuAnim.SetTrigger("Showing");
        isAlive = false;
        GameManager.instance.isBossDead = true;
    }

    public void Respawn()
    {
        HealPlayer(maxHitpoint);
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
        arrows = 10;
    }
}
