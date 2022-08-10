using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InventoryMenu : MonoBehaviour
{
    public Text levelText, hpText, pesosText, upgradeCostText, classText;
    public Image weaponSprite;
    public List<Sprite> sageweapon;
    public RectTransform xpBar;

    //Weapon Upgrade
    public void WeaponUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon())
            switch (GameManager.instance.playerClass)
            {
                case 0:
                    {
                        weaponSprite.sprite = GameManager.instance.swordSprites[GameManager.instance.weaponLevel];
                        break;
                    }
                case 1:
                    {
                        weaponSprite.sprite = GameManager.instance.bowSprites[GameManager.instance.weaponLevel];
                        break;
                    }
                case 2:
                    {
                        weaponSprite.sprite = sageweapon[GameManager.instance.weaponLevel];
                        break;
                    }
            }
        GameManager.instance.save.setWeaponLevel(GameManager.instance.weaponLevel++);
        UpdateMenu();
    }

    //Menu logic
    public void UpdateMenu()
    {
        GameManager.instance.inv.UpdateInventory();
        hpText.text = GameManager.instance.player.hitpoint.ToString();

        if (GameManager.instance.GetLevel() == GameManager.instance.xpTable.Count)
            xpBar.localScale = Vector3.one;
        else
        {
            int prevLvlXp = GameManager.instance.GetXpToLevel(GameManager.instance.GetLevel() - 1);
            int currLvlXp = GameManager.instance.GetXpToLevel(GameManager.instance.GetLevel());

            int diff = currLvlXp - prevLvlXp;
            int currXpIntoLevel = GameManager.instance.xp - prevLvlXp;

            xpBar.localScale = new Vector3((float)currXpIntoLevel / (float)diff, 1, 1);
        }

        levelText.text = GameManager.instance.GetLevel().ToString();
        pesosText.text = GameManager.instance.money.ToString();
        var tempcolor = weaponSprite.color;
        tempcolor.a = 1f;
        weaponSprite.color = tempcolor;

        switch (GameManager.instance.playerClass)
        {
            case 0:
                {
                    if (GameManager.instance.weaponLevel < GameManager.instance.swordSprites.Count - 1)
                    {
                        upgradeCostText.text = "Upgrade: " + GameManager.instance.weaponPrices[GameManager.instance.weaponLevel];
                    }
                    else
                    {
                        upgradeCostText.text = "Max Upgrade!";
                        upgradeCostText.GetComponentInParent<Button>().enabled = false;
                    }
                    classText.text = "Warrior, LVL";
                    weaponSprite.sprite = GameManager.instance.swordSprites[GameManager.instance.weaponLevel];
                    weaponSprite.preserveAspect = true;
                    break;
                }
            case 1:
                {
                    if (GameManager.instance.weaponLevel < GameManager.instance.bowSprites.Count - 1)
                    {
                        upgradeCostText.text = "Upgrade: " + GameManager.instance.weaponPrices[GameManager.instance.weaponLevel];
                    }
                    else
                    {
                        upgradeCostText.text = "Max Upgrade!";
                        upgradeCostText.GetComponentInParent<Button>().enabled = false;
                    }
                    classText.text = "Archer, LVL";
                    weaponSprite.sprite = GameManager.instance.bowSprites[GameManager.instance.weaponLevel];
                    weaponSprite.preserveAspect = true;
                    break;
                }
            case 2:
                {
                    if (GameManager.instance.weaponLevel < GameManager.instance.sageAnims.Count - 1)
                    {
                        upgradeCostText.text = "Upgrade: " + GameManager.instance.weaponPrices[GameManager.instance.weaponLevel];
                    }
                    else
                    {
                        upgradeCostText.text = "Max Upgrade!";
                        upgradeCostText.GetComponentInParent<Button>().enabled = false;
                    }
                    classText.text = "Sage, LVL";
                    weaponSprite.sprite = sageweapon[GameManager.instance.weaponLevel];
                    weaponSprite.preserveAspect = true;
                    break;
                }
            default:
                { Debug.Log("Nonn working :("); break; }
        }
    }

    public void ToMainMenuBtn()
    {
        GameManager.instance.SaveGame(GameManager.instance.save);
        GameManager.instance.hUD.GetComponent<CanvasGroup>().alpha = 0;
        GameManager.instance.invMenu.GetComponent<Animator>().SetBool("hidden", true);
        upgradeCostText.GetComponentInParent<Button>().enabled = true;
        switch (GameManager.instance.playerClass)
        {
            case 0:                    //sword
                {
                    GameManager.instance.audio.Stop("Sword2");
                    break;
                }

            case 1:                    //bow
                {
                    GameManager.instance.audio.Stop("Shoot3");
                    GameManager.instance.player.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    break;
                }
            case 2:                    //staff
                {
                    GameManager.instance.audio.Stop("Magic2");
                    GameManager.instance.player.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    break;
                }
        }
        SceneManager.sceneLoaded += GameManager.instance.LoadGame;
        SceneManager.LoadScene("MainMenu");
    }
}
