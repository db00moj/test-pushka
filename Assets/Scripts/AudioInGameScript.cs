using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInGameScript : MonoBehaviour
{
    [SerializeField] private bool isMusic = false;

    public void Start()
    {
        if (isMusic) if (PlayerPrefs.GetInt("MusicEnabled") < 1) GetComponent<AudioSource>().volume = 0;
        if (isMusic==false) if (PlayerPrefs.GetInt("SoundEnabled") < 1) GetComponent<AudioSource>().volume = 0;
    }
}
