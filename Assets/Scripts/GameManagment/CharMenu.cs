using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharMenu : MonoBehaviour
{
    //New game, so:
    //Set player sprite to one chosen;
    //Set weapon slider + weapon of class;
    //LoadScene("Hub")

    //Put Gamemanager into MainMenu scene, if New Game - reset all data, if not - stays the same
    //New Game - set sprite and animator for character, set HP/Mana etc.
    //Put Mana in Gamemanager for sage, and when new game created create object of sword, bow or staff with their's systems
    
    public void WarrBtn()
    {
        GameManager.instance.save.setClass(0);
        GameManager.instance.save.setWeaponLevel(0);
        GameManager.instance.save.resetMoney(); GameManager.instance.save.resetXp(); GameManager.instance.save.resetInv();
        GameManager.instance.SaveGame(GameManager.instance.save);
        GameManager.instance.player.hitpoint = 5;
        SceneManager.LoadScene("Hub");
    }
    public void ArchBtn()
    {
        GameManager.instance.save.setClass(1);
        GameManager.instance.save.setWeaponLevel(0);
        GameManager.instance.save.resetMoney(); GameManager.instance.save.resetXp(); GameManager.instance.save.resetInv();
        GameManager.instance.SaveGame(GameManager.instance.save);
        GameManager.instance.player.hitpoint = 5;
        SceneManager.LoadScene("Hub");
    }
    public void SageBtn()
    {
        GameManager.instance.save.setClass(2);
        GameManager.instance.save.setWeaponLevel(0);
        GameManager.instance.save.resetMoney(); GameManager.instance.save.resetXp(); GameManager.instance.save.resetInv();
        GameManager.instance.SaveGame(GameManager.instance.save);
        GameManager.instance.player.hitpoint = 5;
        SceneManager.LoadScene("Hub");

    }
}
