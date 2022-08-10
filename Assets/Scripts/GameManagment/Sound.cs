using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    [Range(0f,1f)]
    public float volume;

    [Range(0.1f,3f)]
    public float pitch;

    public string name;
    public bool loop;
    public bool isMusic;

    public AudioMixerGroup mixer;

    [HideInInspector]
    public AudioSource source;
}
