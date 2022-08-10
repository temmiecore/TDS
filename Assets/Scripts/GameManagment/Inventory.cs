using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //Need item list with item types in it
    //Every cell has button, if rightclick - use item, left click - move it?
    //Need to add items to list and cell, and remove it after using
    //Refresh cells everytime menu opens, so UpdateInventory()

    public Transform[] cellsMatrix;

    private void Awake()
    {
        cellsMatrix = new Transform[9];
        for (int i = 0; i < 3; i++)
        {
            cellsMatrix[i] = transform.Find("InventoryCell" + i.ToString());
        }
    }

    public void AddItem(ItemsInventory item)
    {
        for (int i = 0; i < 3; i++)
        {
            bool check = false;
            foreach (ItemsInventory it in GameManager.instance.items)
                if (it != null && it.position == i)
                { check = true; break; }
            if (check == false)
            {
                item.position = i;
                GameManager.instance.save.addItem(item, i);
                cellsMatrix[i].GetComponent<Image>().sprite = item.sprite;
                cellsMatrix[i].GetComponent<Button>().enabled = true;
                UpdateInventory();
                Debug.Log("Item added, position " + i);
                return;
            }
        }
        Debug.Log("No place in inventory!");
    }
    public void RemoveItem(int cellNum) //Remove, use
    {
        cellsMatrix[cellNum].GetComponent<Image>().sprite = null;
        cellsMatrix[cellNum].GetComponent<Button>().enabled = false;
        GameManager.instance.save.removeItem(cellNum);
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < 3; i++)
        {
            cellsMatrix[i].GetComponent<Image>().sprite = null;
            var tempcolor = cellsMatrix[i].GetComponent<Image>().color;
            tempcolor.a = 0f;
            cellsMatrix[i].GetComponent<Image>().color = tempcolor;
            cellsMatrix[i].GetComponent<Button>().enabled = false;
        }
        foreach (ItemsInventory item in GameManager.instance.items)
        {
            if (item != null)
            {
                cellsMatrix[item.position].GetComponent<Image>().sprite = item.sprite;
                var tempcolor = cellsMatrix[item.position].GetComponent<Image>().color;
                tempcolor.a = 1f;
                cellsMatrix[item.position].GetComponent<Image>().color = tempcolor;
                cellsMatrix[item.position].GetComponent<Button>().enabled = true;
            }
        }   
    }

    public void OnItemClick(GameObject cell)
    {
        int cellNum = int.Parse(cell.name[cell.name.Length - 1].ToString());
        Debug.Log("Item trying to be used, cell" + cellNum);
        switch (GameManager.instance.items[cellNum].itemType)
        {
            case ItemsFloor.ItemType.healthPot:
                {
                    if (GameManager.instance.player.hitpoint != GameManager.instance.player.maxHitpoint)
                    { GameManager.instance.player.HealPlayer(5); RemoveItem(cellNum); }
                    break; 
                }
            //Add more for every item that can be used
        }
    }
}
