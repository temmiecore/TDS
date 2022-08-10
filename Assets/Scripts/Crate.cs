using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter
{
    public List<GameObject> prefabsList;
    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.DropItems(1, prefabsList, gameObject);
    }

}
