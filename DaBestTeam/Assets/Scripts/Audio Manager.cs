using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----Component----")]
    public static AudioManager instance;
   


    [Header("----Player Audio----")]
    [SerializeField] AudioSource audioPlayer;
    [SerializeField] AudioClip[] playerSteps;
    [Range(0, 1)][SerializeField] float playerStepVol;
    [SerializeField] AudioClip[] soundHurt;
    [Range(0, 1)][SerializeField] float soundHurtVol;

    [Header("----Weapon Audio----")]
    [SerializeField] AudioSource audioWeapon;
    [SerializeField] AudioClip[] playerShoot;
    [Range(0, 1)][SerializeField] float playerShootVol;



    [Header("----Level Audio----")]
    [SerializeField] AudioClip elevatorArrived;
    [Range(0, 1)][SerializeField] float elevatorArrivedVol;

    public bool isPlayingSteps;
    bool isSprinting;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optionally, to keep AudioManager persistent across scenes
        }
        else
        {
            Destroy(gameObject); // If another AudioManager instance already exists, destroy this one
        }
    }


    public void hurtSound()
    {
        audioPlayer.PlayOneShot(soundHurt[Random.Range(0, soundHurt.Length)], soundHurtVol);
    }

    public void playFootSteps()
    {
        Debug.Log("Calling Audio Method");
        isPlayingSteps = true;

        audioPlayer.PlayOneShot(playerSteps[Random.Range(0, (playerSteps.Length))], playerStepVol);

        isPlayingSteps = false;
    }

    public void shootSound()
    {
        audioWeapon.PlayOneShot(playerShoot[Random.Range(0, (playerShoot.Length))], playerShootVol);
    }
}
