using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform playerlookup;
    public float boundX = 0.3f, boundY = 0.15f;

    private void Start()
    {
        playerlookup = GameObject.Find("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 distance = Vector3.zero;
        float distX = playerlookup.position.x - transform.position.x;
        if (distX > boundX || distX < -boundX)
        {
            if (transform.position.x < playerlookup.position.x )
                distance.x = distX - boundX;
            else
                distance.x = distX + boundX;
        }
        float distY = playerlookup.position.y - transform.position.y;
        if (distY > boundY || distY < -boundY)
        {
            if (transform.position.y < playerlookup.position.y )
                distance.y = distY - boundY;
            else
                distance.y = distY + boundY;
        }
        transform.position += new Vector3(distance.x, distance.y, 0);
    }
}
