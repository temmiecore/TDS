using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject instPrefab;
    public GameObject dialgtPrefab;

    public void Show(string msg, Color color, Vector3 position, int type) //1 is instant, 0 is dialogue
    {
        if (type == 0)
        {
            GameObject newText = Instantiate(dialgtPrefab, position, Quaternion.identity);
            newText.GetComponentInChildren<TextMeshPro>().text = msg;
            newText.GetComponentInChildren<TextMeshPro>().color = color;
        }
        else
        {
            GameObject newText = Instantiate(instPrefab, position, Quaternion.identity);
            newText.GetComponentInChildren<TextMeshPro>().text = msg;
            newText.GetComponentInChildren<TextMeshPro>().color = color;
        }
           

    }
}
