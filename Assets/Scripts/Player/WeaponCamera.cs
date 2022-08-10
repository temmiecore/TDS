using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCamera : MonoBehaviour
{
    void Update()
    {
        Vector3 cursorpos = Input.mousePosition;
        cursorpos = Camera.main.ScreenToWorldPoint(cursorpos);
        Vector2 direction = new Vector2(cursorpos.x - transform.position.x, cursorpos.y - transform.position.y);
        transform.right = direction;
    }
}
