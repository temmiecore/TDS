using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsFloor : Collectable
{
    //Items have type, which defines what they will do on collision
    //Coin += pesos
    //Potions and weapons go to inventory, potions give effect and if move weapon to weapon slot = ChangeWeapon()
    //Arrows go to inventory, but you can't move them or use, they stack

    public enum ItemType
    {
        coin,
        healthPot,
        manaPot,
        boostPot,
        sword,
        staff,
        bow,
        arrow
    }

    private float delay;
    public ItemType itemType;
    public Sprite sprite;


    protected override void Start()
    {
        base.Start();
        delay = Time.realtimeSinceStartup;
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (!isCollected && Time.realtimeSinceStartup - delay > 1f && coll.tag == "Player")
        {
            switch (itemType)
            {
                case ItemType.coin:
                    {
                        isCollected = true;
                        GameManager.instance.save.addMoney(2);
                        GameManager.instance.Showtext("*", Color.yellow,
                new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 0.1f, 0),1);

                        break;
                    }
                case ItemType.healthPot:
                    {
                        isCollected = true;
                        ItemsInventory item = new ItemsInventory();
                        item.itemType = itemType;
                        item.sprite = sprite;
                        GameManager.instance.inv.AddItem(item);
                        GameManager.instance.Showtext("*", Color.green,
                new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 0.1f, 0),1);

                        break;
                    }
                case ItemType.manaPot:
                        {
                            ItemsInventory item = new ItemsInventory();
                            item.itemType = itemType;
                            item.sprite = sprite;
                            GameManager.instance.inv.AddItem(item);
                            break;
                        }
                /*case ItemType.boostPot:
                    {
                        ItemsInventory item = new ItemsInventory();
                        item.itemType = this.itemType;
                        item.sprite = sprite;
                        GameManager.instance.inv.AddItem(item);
                        break;
                    }*/
            }
            Destroy(gameObject);
        }
    }

}
