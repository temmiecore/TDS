using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int money;
    public int xp;
    public int playerClass;
    public int weaponLevel;
    public int[,] items;
    public int hp;

    public void addMoney(int pesos) { money += pesos; GameManager.instance.money += pesos; }
    public void addXp(int xp) { this.xp += xp; GameManager.instance.xp += xp; }
    public void setClass(int plclass) { playerClass = plclass; GameManager.instance.playerClass = plclass; }
    public void setWeaponLevel(int level) { weaponLevel = level; GameManager.instance.weaponLevel = level; }
    public void addItem(ItemsInventory item, int cell)
    {
        if (items == null)
            items = new int[8, 9];
        items[((int)item.itemType), cell] = 1; GameManager.instance.items[cell] = item;
    }
    public void changeHp(int hp) { this.hp += hp; GameManager.instance.player.hitpoint += hp; }

    public void resetMoney() { money = 0; GameManager.instance.money = 0; }
    public void resetXp() { xp = 0; GameManager.instance.xp = 0; }
    public void resetInv() { items = default; GameManager.instance.items = default; }
    public void removeItem(int cell)
    {
        for (int i = 0; i < 8; i++)
            items[i, cell] = 0;
        GameManager.instance.items[cell] = null;
    }
}
