using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    public Transform container;
    Image barImage;
    public Image contImage;

    void Start()
    {
        barImage = GetComponent<Image>();
    }

    public void UpdateContainer(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0)
            currentHP = 0;
        Debug.Log(damage + ", " + currentHP + ", " + maxHP);
        container.transform.localScale = new Vector3(((float)currentHP / (float)maxHP),1,1);
    }

    float maxHP;
    float currentHP;
    public void Initialize(float hp)
    {
        ChangeAlpha(1f);
        maxHP = hp;
        currentHP = maxHP;
        UpdateContainer(0);
    }

    public void Died()
    {
        ChangeAlpha(0f);
    }

    void ChangeAlpha(float a)
    {
        var tempColor = barImage.color;
        tempColor.a = a;
        barImage.color = tempColor;
        tempColor = contImage.color;
        tempColor.a = a;
        contImage.color = tempColor;
    }
}
