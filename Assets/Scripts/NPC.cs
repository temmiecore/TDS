using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Collidable
{
    public float lastCall = -5f, timeToSpeak = 7f;
    string[] messages = new string[4];
    int messagenum = 0;
    public int NPCnum;
    public Animator anim;

    public RuntimeAnimatorController warriorAnim;
    public RuntimeAnimatorController archAnim;
    public RuntimeAnimatorController sageAnim;

    protected override void Start()
    {
        base.Start();
        switch (GameManager.instance.playerClass)
        {
            case 0: //Warrior
                {
                    if (NPCnum == 0)  //Speaks to archer
                    { 
                        SetSprite(archAnim);
                        messages[0] = "There you are. {something something}";
                        messages[1] = "Good luck, warrior!";
                        messages[2] = "What, you again?\nBe off with you, already! (?)";
                    }
                    else
                    { 
                        SetSprite(sageAnim); //Speaks to sage

                        messages[0] = "Do not come back\nwithout something new.";
                        messages[1] = "We cannot afford\nanother failed expedition!";
                        messages[2] = "...";
                    }
                    break; 
                }
            case 1: //Arch
                {
                    if (NPCnum == 0) //Speaks to warr
                    { 
                        SetSprite(warriorAnim);
                        messages[0] = "Godspeed, boy.";
                        messages[1] = "I will pray foryour luck (?)";
                    }
                    else
                    { 
                        SetSprite(sageAnim); //Speaks to sage
                        messages[0] = "You are the fastest, elf.";
                        messages[1] = "Let's hope swiftness\nis the answer.";
                        messages[1] = "...";
                    }
                    break; 
                }
            case 2: //Sage
                {
                    if (NPCnum == 0) //Speaks to warr
                    {
                        SetSprite(warriorAnim);
                        messages[0] = "...";
                        messages[1] = "...";
                        messages[2] = "..."; 
                    }
                    else
                    {
                        SetSprite(archAnim); //Speaks to archer
                        messages[0] = "Are you a cleric or something?";
                        messages[1] = "Time to take the job into\nyour own hands, huh?";
                        messages[2] = "Best luck with your pilgrimage,\nor whatever you do, ha-ha!";
                        messages[3] = "What, you again?\nBe off with you, already! (?)";
                    }
                    break; 
                }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
       if (coll.name == "Player")
        {
            if (Time.time - lastCall > timeToSpeak)
            {
                lastCall = Time.time;
                GameManager.instance.Showtext(messages[messagenum],Color.white,
                   new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 0.1f, 0), 0);
                if (messagenum < messages.Length-1)
                    messagenum++;
            }
        }
    }

    public void SetSprite(RuntimeAnimatorController animatorC)
    {
        anim.runtimeAnimatorController = animatorC;
    }

    public float idleSpeed;
    public SpriteRenderer moverSprite;

    float idledelay;
    bool stopTrigger = false;
    float randX, randY;

    IEnumerator Wait()
    {
        stopTrigger = true;
        randX = Random.Range(-1f, 1f);
        randY = Random.Range(-1f, 1f);
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        stopTrigger = false;
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup - idledelay > Random.Range(4f, 7f))
        { StartCoroutine(Wait()); idledelay = Time.realtimeSinceStartup; }

        if (stopTrigger)
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + randX, transform.position.y + randY), idleSpeed * Time.deltaTime);

        if (randX < 0)
            moverSprite.flipX = true;
        else if (randX > 0)
            moverSprite.flipX = false;
    }
}
