using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable
{
    public string[] sceneNames;

    protected override void OnCollide(Collider2D coll)
    {
        string sceneName;
        if (coll.name == "Player" && GameManager.instance.isBossDead == true)
        {
            GameManager.instance.SaveGame(GameManager.instance.save);
            sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            SceneManager.LoadScene(sceneName);
        }
    }
}
