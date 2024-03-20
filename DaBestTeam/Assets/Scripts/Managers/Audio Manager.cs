using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds, zombieSounds;
    public AudioSource musicSource, sfxSource;
    public AudioSource zombieSource;

    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider zombieSlider;

    private void Start()
    {
            instance = this;
        if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolumeSetting();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
            SetZombieSFXVolume();
        }

        GameObject zombieGameObject = GameObject.FindGameObjectWithTag("Enemy");

        if (zombieGameObject != null)
        {
            // Get the ZombieAI component from the GameObject
            ZombieAI zombieAI = zombieGameObject.GetComponent<ZombieAI>();

            if (zombieAI != null)
            {
                // Get the AudioSource from the ZombieAI component
                zombieSource = zombieAI.zombieSource;
            }
            else
            {
                Debug.Log("component not found ");
            }
        }
        else
        {
            Debug.Log("Enemy not found.");
        }

        PlayMusic("Theme");

    }
    public void Update()
    {      
            
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
            
        }
    }
    public void PlayZombieSFX(string name)
    {
        Sound s = Array.Find(zombieSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Not Found");
        }
        else
        {
            zombieSource.PlayOneShot(s.clip);            
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void ToggleZombieSFX()
    {
        zombieSource.mute = !zombieSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
    public void ZombieSFXVolume(float volume)
    {
        zombieSource.volume = volume;
    }
    public void LoadVolumeSetting()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        zombieSlider.value = PlayerPrefs.GetFloat("zombieVolume");
        SetMusicVolume();
        SetSFXVolume();
        SetZombieSFXVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
    public void SetZombieSFXVolume()
    {
        float volume = zombieSlider.value;
        myMixer.SetFloat("zombie", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("zombieVolume", volume);
    }
}
