using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public SaveData save = new SaveData();

    private void Awake()
    {
        filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "/save.data";
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingText.gameObject);
            Destroy(hUD.gameObject);
            Destroy(invMenu.gameObject);
            Destroy(audio.gameObject);
            return;
        }
        instance = this;
        isBossDead = true;
        SceneManager.sceneLoaded += OnLoadScene;
        SceneManager.sceneLoaded += LoadGame;
    }

    //Resources
    public List<int> xpTable;
    public List<Sprite> swordSprites;
    public List<Sprite> bowSprites;
    public List<int> weaponPrices;
    public List<Sprite> playerClasses;
    public List<RuntimeAnimatorController> sageAnims;
    public List<Sprite> itemSprites;
    public List<int> weaponClasses;
    private string filePath;


    //References
    public Player player;
    public FloatingTextManager floatingText;
    public Sword sword;
    public Bow bow;
    public Staff staff;
    public Knife knife;
    public RectTransform hpBar;
    public GameObject hUD;
    public GameObject invMenu;
    public Animator deathMenuAnim;
    public AudioManager audio;
    public RectTransform manaBar;
    public GameObject manaBarBorder;
    public Inventory inv;
    public RuntimeAnimatorController warriorAnim;
    public RuntimeAnimatorController archAnim;
    public RuntimeAnimatorController sageAnim;
    public BossHP bossHP;

    //Saving Data
    public int money;
    public int xp;
    public int playerClass;
    public int weaponLevel;
    public ItemsInventory[] items;

    public bool isBossDead;
    public bool isKnife;

    //Floating text logic - it's here to use it from everywhere without adressing FloatingTextManager.cs every time!
    public void Showtext(string msg, Color color, Vector3 position, int type)
    {
        floatingText.Show(msg, color, position, type);
    }

    // To Save:
    // Skin, EXP, Money, Weapon, Mana, Hp (?)
    public void SaveGame(SaveData saveData)
    {
        FileStream dataStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, saveData);

        dataStream.Close();
        Debug.Log("dataStream closed, game saved.");
    }

    public void OnLoadScene(Scene s, LoadSceneMode mode)
    {
        AudioManager audiomanager = FindObjectOfType<AudioManager>();
        player.transform.position = GameObject.Find("Spawn").transform.position;
        Debug.Log("Player spawned");

        switch (s.name)
        {
            case "MainMenu":
                {
                    audiomanager.StopPlayingMusic(); audiomanager.Play("Title"); player.isInMenu = true;
                    sword.isInMenu = true; staff.isInMenu = true; bow.isInMenu = true; break;
                }
            case "Hub":
                {
                    player.isInMenu = false; sword.isInMenu = false; staff.isInMenu = false; bow.isInMenu = false;
                    audiomanager.StopPlayingMusic(); audiomanager.Play("Hub"); break;
                }
            case "DungeonLvl1":
                {
                    audiomanager.StopPlayingMusic(); audiomanager.Play("Dung1"); isBossDead = false; break;
                }
        }
    }

    public void LoadGame(Scene s, LoadSceneMode mode)
    {
        if (s.name != "MainMenu")
        {

            if (!File.Exists(filePath))
                return;

            FileStream dataStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);

            BinaryFormatter converter = new BinaryFormatter();
            SaveData saveData = converter.Deserialize(dataStream) as SaveData;

            dataStream.Close();
            SceneManager.sceneLoaded -= LoadGame;

            CanvasGroup canvasgroup = hUD.GetComponent<CanvasGroup>();
            canvasgroup.alpha = 1;


            playerClass = saveData.playerClass;

            if (saveData.hp != 0)
                player.hitpoint = saveData.hp;
            HPchange();

            switch (playerClass)
            {
                case 0:                    //sword
                    {
                        player.SetSprite(playerClass, warriorAnim);
                        player.speedX = 0.9f; player.speedY = 0.7f;
                        manaBarBorder.GetComponent<Image>().enabled = false;
                        manaBar.GetComponent<Image>().enabled = false;
                        break;
                    }

                case 1:                    //bow
                    {
                        player.SetSprite(playerClass, archAnim);
                        player.speedX = 1f; player.speedY = 0.8f;
                        manaBarBorder.GetComponent<Image>().enabled = false;
                        manaBar.GetComponent<Image>().enabled = false;
                        break;
                    }
                case 2:                    //staff
                    {
                        player.SetSprite(playerClass, sageAnim);
                        player.speedX = 0.75f; player.speedY = 0.60f;
                        manaBarBorder.GetComponent<Image>().enabled = true;
                        manaBar.GetComponent<Image>().enabled = true;
                        break;
                    }
            }

            money = saveData.money;
            xp = saveData.xp;
            if (GetLevel() != 1)
                player.SetLevel(GetLevel());

            if (saveData.items != null)
            {
                items = new ItemsInventory[3];
                for (int i = 0; i < 8; i++) //types
                    for (int j = 0; j < 3; j++) //cells
                        if (saveData.items[i, j] != 0)
                        {
                            items[j] = new ItemsInventory();
                            items[j].itemType = (ItemsFloor.ItemType)i;
                            items[j].sprite = itemSprites[i - 1];
                        }

            }
            else
                items = new ItemsInventory[3];

            inv.UpdateInventory();

            weaponLevel = saveData.weaponLevel;
            switch (playerClass)
            {
                case 0:                    //sword
                    {
                        sword.gameObject.SetActive(true);
                        bow.gameObject.SetActive(false);
                        staff.gameObject.SetActive(false);
                        sword.spriteRenderer.sprite = swordSprites[weaponLevel];
                        knife.gameObject.SetActive(false);
                        break;
                    }
                case 1:                    //bow
                    {
                        bow.gameObject.SetActive(true);
                        sword.gameObject.SetActive(false);
                        staff.gameObject.SetActive(false);
                        bow.spriteRenderer.sprite = bowSprites[weaponLevel];
                        break;
                    }
                case 2:                    //staff
                    {
                        staff.gameObject.SetActive(true);
                        bow.gameObject.SetActive(false);
                        sword.gameObject.SetActive(false);
                        player.SetSprite(sageAnims[weaponLevel]);
                        knife.gameObject.SetActive(false);
                        break;
                    }
            }
            Debug.Log("Game loaded.");
        }
    }


    //Weapon Upgrade
    public bool TryUpgradeWeapon()
    {
        if (weaponLevel >= weaponPrices.Count)
            return false;
        if (money >= weaponPrices[weaponLevel])
        {
            save.addMoney(-weaponPrices[weaponLevel]);
            if (playerClass == 0)
                sword.Upgrade();
            else if (playerClass == 1)
                bow.Upgrade();
            else
                staff.Upgrade();
            return true;
        }
        return false;
    }

    //Leveling System
    public int GetLevel()
    {
        int returnValue = 0, toAdd = 0;

        while (xp >= toAdd)
        {
            toAdd += xpTable[returnValue];
            returnValue++;
            if (returnValue == xpTable.Count)
                return returnValue;
        }

        return returnValue;
    }

    public void GainXp(int xpFromMob)
    {
        int currLevel = GetLevel();
        save.addXp(xpFromMob);
        if (currLevel < GetLevel())
            LevelUp();
    }

    public int GetXpToLevel(int level)
    {
        int r = 0, exp = 0;
        while (r < level)
        {
            exp += xpTable[r];
            r++;
        }
        return exp;
    }

    public void LevelUp()
    {
        Showtext("LVL UP!", Color.white,
                new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 0.1f, 0), 1);

        player.OnLevelUp();
        HPchange();
    }

    public void HPchange()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
        hpBar.localScale = new Vector3(1, ratio, 1);
    }

    public void ManaChange()
    {
        float ratio = (float)player.mana / (float)player.maxMana;
        manaBar.localScale = new Vector3(1, ratio, 1);
    }

    public void Respawn()
    {
        SceneManager.LoadScene("Hub");
        deathMenuAnim.SetTrigger("Hiding");
        save.resetXp(); GetLevel();
        save.setWeaponLevel(0);
        if (playerClass == 0)
            sword.spriteRenderer.sprite = swordSprites[0];
        else if (playerClass == 1)
            bow.spriteRenderer.sprite = bowSprites[0];
        else
            player.SetSprite(sageAnim);
        save.resetMoney();
        player.maxHitpoint = 5;
        bossHP.Died();
        player.Respawn();
    }

    public void DropItems(int num, List<GameObject> prefabsList, GameObject spawn)
    {
        if (prefabsList != null && prefabsList.Count != 0)
        {
            for (int i = 0; i < num; i++)
            {
                if (Random.Range(0f, 1f) > 0.6f)
                {
                    switch (playerClass)
                    {
                        case 0:
                            {
                                Instantiate(prefabsList[Random.Range(0, prefabsList.Count - 1)],
                                  new Vector3(spawn.transform.position.x, spawn.transform.position.y - 0.1f, 0),
                                   Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.8f, 0.8f), Random.Range(-1f, 1f)) * 3f);
                                break;
                            }
                        case 1:
                            {
                                Instantiate(prefabsList[Random.Range(0, prefabsList.Count - 1)],
                                  new Vector3(spawn.transform.position.x, spawn.transform.position.y - 0.1f, 0),
                                   Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.8f, 0.8f), Random.Range(-1f, 1f)) * 3f);
                                break;
                            }
                        case 2:
                            {
                                Instantiate(prefabsList[Random.Range(1, prefabsList.Count)],
                                     new Vector3(spawn.transform.position.x, spawn.transform.position.y - 0.1f, 0),
                                      Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.8f, 0.8f), Random.Range(-1f, 1f)) * 3f);
                                break;
                            }
                    }
                }
            }
        }
    }
}
