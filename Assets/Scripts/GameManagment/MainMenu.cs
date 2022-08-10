using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public void LoadButton()
    {
        if (!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "/save.data"))
            return;
        SceneManager.LoadScene("Hub");
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
