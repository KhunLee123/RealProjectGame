using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer myMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        float savedMusic = PlayerPrefs.GetFloat("SavedMusicVol", 1f);
        float savedSFX = PlayerPrefs.GetFloat("SavedSFXVol", 1f);

        if (musicSlider != null) musicSlider.value = savedMusic;
        if (sfxSlider != null) sfxSlider.value = savedSFX;

        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);
    }

    public void SetMusicVolume(float sliderValue)
    {
        myMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        
        PlayerPrefs.SetFloat("SavedMusicVol", sliderValue); 
    }

    public void SetSFXVolume(float sliderValue)
    {
        myMixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
        
        PlayerPrefs.SetFloat("SavedSFXVol", sliderValue); 
    }
}