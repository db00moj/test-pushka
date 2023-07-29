using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private Image soundIcon;
    [SerializeField] private Image musicIcon;
    private int musicEnabled = 1;
    private int soundEnabled = 1;
    void Start()
    {
        if (PlayerPrefs.HasKey("MusicEnabled")) musicEnabled = PlayerPrefs.GetInt("MusicEnabled");
        if (PlayerPrefs.HasKey("SoundEnabled")) soundEnabled = PlayerPrefs.GetInt("SoundEnabled");

        Application.targetFrameRate = 60;

        GameObject.Find("LastScoreText").GetComponent<Text>().text = "LAST SCORE: " + PlayerPrefs.GetInt("LastScore");
        GameObject.Find("BestScoreText").GetComponent<Text>().text = "BEST SCORE: " + PlayerPrefs.GetInt("BestScore");

        SwitchMusic(); SwitchMusic();
        SwitchSound(); SwitchSound();
    }

    public void SwitchMusic()
    {
        if (musicEnabled == 1) { musicEnabled = 0; musicAudio.volume = 0; musicIcon.color = Color.black; }
        else {musicEnabled = 1; musicAudio.volume = 0.75f;musicIcon.color = Color.white; }
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled);


    }

    public void SwitchSound()
    {
        if (soundEnabled == 1) { soundEnabled = 0; soundIcon.color = Color.black; }
        else { soundEnabled = 1; soundIcon.color = Color.white; }
        PlayerPrefs.SetInt("SoundEnabled", soundEnabled);
    }

    public void GoToScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void ExitFromGame()
    {
        Application.Quit();
    }
}
