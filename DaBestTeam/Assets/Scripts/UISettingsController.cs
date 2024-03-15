using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;

    bool isPlaying;
    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        AudioManager.instance.PlaySFX("Buzz");
        AudioManager.instance.MusicVolume(_musicSlider.value);       
    }

    public void SFXVolume()
    {
        AudioManager.instance.PlaySFX("Buzz");
        AudioManager.instance.SFXVolume(_sfxSlider.value);
    }

  
}
