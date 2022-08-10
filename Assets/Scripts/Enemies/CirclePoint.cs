using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePoint : MonoBehaviour
{
    private Transform playerTr;

    private void Start()
    {
        playerTr = GameManager.instance.player.transform;
    }

    void Update()
    {
        transform.position = playerTr.position + new Vector3(-Mathf.Cos(Time.time * 0.9f) * 4.7f * 0.16f, Mathf.Sin(Time.time * 0.9f) * 4.7f * 0.16f, 0);
    }
}
