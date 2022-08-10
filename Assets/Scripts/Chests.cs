using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chests : Collectable
{
    public Sprite emptyChest;
    public List<GameObject> prefabsList;

    protected override void OnCollide(Collider2D coll)
    {
        if (!isCollected && coll.name == "Player")
        {
            isCollected = true;


            //Make random objects pop out of chest
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.DropItems(3, prefabsList, gameObject);
        }
    }
}
