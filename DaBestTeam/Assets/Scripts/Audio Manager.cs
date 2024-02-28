using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----Component----")]
    public static AudioManager instance;
   


    [Header("----Player Audio----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] playerSteps;
    [Range(0, 1)][SerializeField] float playerStepVol;

    [SerializeField] AudioClip[] soundHurt;
    [Range(0, 1)][SerializeField] float soundHurtVol;

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
        aud.PlayOneShot(soundHurt[Random.Range(0, soundHurt.Length)], soundHurtVol);
    }

    public void playFootSteps()
    {
        Debug.Log("Calling Audio Method");
        isPlayingSteps = true;

        aud.PlayOneShot(playerSteps[Random.Range(0, (playerSteps.Length))], playerStepVol);

        isPlayingSteps = false;
    }

    public void shootSound()
    {
        aud.PlayOneShot(playerShoot[Random.Range(0, (playerShoot.Length))], playerShootVol);
    }
}
