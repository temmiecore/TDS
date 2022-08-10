using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private Collider2D[] colliders = new Collider2D[10];

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void FixedUpdate()
    {
        boxCollider.OverlapCollider(filter, colliders);
        for (int  i = 0;  i < colliders.Length;  i++)
        {
            if (colliders[i] == null)
                continue;
            OnCollide(colliders[i]);
            colliders[i] = null;
        }
    }

    protected virtual void OnCollide(Collider2D coll)
    {

    }   

}
