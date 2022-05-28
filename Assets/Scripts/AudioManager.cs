using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource musicSource;
    
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
        }
        else
        {
            musicSource.Pause();
        }

        SaveData();
    }

    public void ToogleAudio(){
        toogleAudio = !toogleAudio;

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
    }
}
