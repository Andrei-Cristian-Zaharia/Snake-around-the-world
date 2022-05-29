using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource musicSource;

    public Sprite music;
    public Sprite musicMute;
    
    public Sprite sound;
    public Sprite soundMute;

    public Button musicButton;
    public Button soundButton;

    public static bool toogleMusic = true;
    public static bool toogleAudio = true;

    private void Start()
    {
        LoadData();
        
        musicSource = this.GetComponent<AudioSource>();
    
        if (toogleMusic)
        {
            musicSource.Play();
        }
    }

    public void ToogleMusic()
    {
        toogleMusic = !toogleMusic;
        
        if (toogleMusic)
        {
            musicSource.UnPause();

            musicButton.image.sprite = music;
        }
        else
        {
            musicSource.Pause();

            musicButton.image.sprite = musicMute;
        }

        SaveData();
    }

    public void ToogleAudio(){
        toogleAudio = !toogleAudio;


        if (toogleAudio)
            soundButton.image.sprite = sound;
        else
            soundButton.image.sprite = soundMute;

        SaveData();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Music", toogleMusic ? 1 : 0);
        PlayerPrefs.SetInt("Audio", toogleAudio ? 1 : 0);
    }
    
    private void LoadData()
    {
        toogleMusic = PlayerPrefs.GetInt("Music") == 1;
        toogleAudio = PlayerPrefs.GetInt("Audio") == 1;

        if (toogleMusic)
            musicButton.image.sprite = music;
        else
            musicButton.image.sprite = musicMute;

        if (toogleAudio)
            soundButton.image.sprite = sound;
        else
            soundButton.image.sprite = soundMute;
    }
}
