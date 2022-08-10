using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movers : Fighter
{
    protected BoxCollider2D boxCollider; //Init components and...
    protected Vector3 moveDistance;
    protected RaycastHit2D hit;
    protected SpriteRenderer moversprite;
    public GameObject sword, staff, bow, knife, weaponcam;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>(); //...Link them to Unity components
        moversprite = GetComponent<SpriteRenderer>();
    }
    protected virtual void UpdateMotor(Vector3 input)
    {
        moveDistance = input; //Distance between frames

        if (gameObject.tag == "Player")
        {
            if (sword.activeSelf || knife.activeSelf)
            {
                if (moveDistance.x < 0)
                {
                    moversprite.flipX = true;
                    weaponcam.transform.localScale = new Vector3(1, -1, 1);
                }
                else if (moveDistance.x > 0)
                {
                    moversprite.flipX = false;
                    weaponcam.transform.localScale = Vector3.one;
                }
            }
            else if (staff.activeSelf|| bow.activeSelf)
            {
                if (moveDistance.x < 0)
                    moversprite.flipX = true;
                else if (moveDistance.x > 0)
                    moversprite.flipX = false;
            }
        }
        else
        {
            if (moveDistance.x < 0)
                moversprite.flipX = true;
            else if (moveDistance.x > 0)
                moversprite.flipX = false;
        }

        moveDistance += pushDirection;
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushrecovery);


        //Hit detection
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDistance.y), Mathf.Abs(moveDistance.y * Time.deltaTime), LayerMask.GetMask("Actors", "Blocking")); //For y axis
        if (hit.collider == null)
        { transform.Translate(0, moveDistance.y * Time.deltaTime, 0); }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDistance.x, 0), Mathf.Abs(moveDistance.x * Time.deltaTime), LayerMask.GetMask("Actors", "Blocking")); // x
        if (hit.collider == null)
        { transform.Translate(moveDistance.x * Time.deltaTime, 0, 0); }
    }

   
}
