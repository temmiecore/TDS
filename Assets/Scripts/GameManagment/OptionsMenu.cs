using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Resolution[] resolutions;
    public Dropdown dropdown;

    private void Start()
    {
        resolutions = Screen.resolutions;
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        int currresID = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {

            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currresID = i;
            }
        }
        
        dropdown.AddOptions(options);
        dropdown.value = currresID;
        dropdown.RefreshShownValue();
    }

    public void ChangeVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", volume);
    }

    public void ChangeDisplay(int resID)
    {
        Resolution res = resolutions[resID];
        Screen.SetResolution(res.width, res.height, false);
    }

    public void ChangeFullscreen(bool value)
    {
        if (value == true)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else
            Screen.fullScreenMode = FullScreenMode.Windowed;
    }
}
