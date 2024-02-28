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
    [Range(0, 1)][SerializeField] float playerStepSpeed;
    [Range(0, 1)][SerializeField] float playerSprintSpeed;
    [SerializeField] AudioClip[] soundHurt;
    [Range(0, 1)][SerializeField] float soundHurtVol;

    [Header("----Weapon PickUp Audio----")]
    [SerializeField] AudioSource audioWeapon;
    [SerializeField] AudioClip[] weaponShoot;
    [Range(0, 1)][SerializeField] float weaponShootVol;
    [SerializeField] AudioClip[] weaponReload;
    [Range(0, 1)][SerializeField] float weaponReloadVol;

    [Header("----Level Audio----")]
    [SerializeField] AudioSource audioElevator;    
    [SerializeField] AudioClip[] elevatorArrived;
    [Range(0, 1)][SerializeField] float elevatorArrivedVol;


    [Header("----Boss Enemy Audio----")]
    [SerializeField] AudioSource audioEnemy;
    [SerializeField] AudioClip[] enemySteps;
    [Range(0, 1)][SerializeField] float enemyStepVol;
    [Range(0, 1)][SerializeField] float enemyStepSpeed;
    [Range(0, 1)][SerializeField] float enemySprintSpeed;
    [SerializeField] AudioClip[] enemySoundHurt;
    [Range(0, 1)][SerializeField] float enemySoundHurtVol;
    [SerializeField] AudioClip[] enemyShoot;
    [Range (0, 1)][SerializeField] float enemyShootVol;

    [Header("----Elite Enemy Audio----")]
    [SerializeField] AudioSource eliteAudioEnemy;

    [Header("----Medium Enemy Audio----")]
    [SerializeField] AudioSource mediumAudioEnemy;

    [Header("----Basic Enemy Audio----")]
    [SerializeField] AudioSource basicAudioEnemy;

    bool isPlayingSteps;
    bool isSprinting;
    bool isPlayingEnemySteps;


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
        if (!isPlayingSteps)
        {
            StartCoroutine(ResetIsPlayingSteps());
        }
        
    }

    IEnumerator ResetIsPlayingSteps()
    {
        isPlayingSteps = true; 

        float delay = isSprinting ? playerStepSpeed : playerSprintSpeed; 
        yield return new WaitForSeconds(delay); 

        int randomIndex = Random.Range(0, playerSteps.Length); 
        audioPlayer.PlayOneShot(playerSteps[randomIndex], playerStepVol); 

        yield return new WaitForSeconds(playerSteps[randomIndex].length); 

        isPlayingSteps = false;

    }

    public void shootSound()
    {
        audioWeapon.PlayOneShot(weaponShoot[Random.Range(0, (weaponShoot.Length))], weaponShootVol);
    }

    public void reloadSound()
    {
        audioWeapon.PlayOneShot(weaponReload[Random.Range(0, weaponReload.Length)], weaponReloadVol);
    }

    public void enemyStepSound()
    {
        if (!isPlayingEnemySteps) 
        {
            StartCoroutine(ResetIsPlayingEnemySteps());
        }
    }

    IEnumerator ResetIsPlayingEnemySteps()
    {
        isPlayingEnemySteps = true;

        float delay = isSprinting ? enemyStepSpeed : enemySprintSpeed;
        yield return new WaitForSeconds(delay);

        int randomIndex = Random.Range(0, enemySteps.Length);
        audioEnemy.PlayOneShot(enemySteps[randomIndex], enemyStepVol);

        yield return new WaitForSeconds(enemySteps[randomIndex].length);

        isPlayingEnemySteps = false;

    }

    public void enemyHurtSound()
    {
        audioEnemy.PlayOneShot(enemySoundHurt[Random.Range(0, enemySoundHurt.Length)], enemySoundHurtVol);
    }

    public void enemyShootSound()
    {
        audioEnemy.PlayOneShot(enemyShoot[Random.Range(0, enemyShoot.Length)], enemyShootVol);
    }

    public void elevatorArrivedSound()
    {
        audioElevator.PlayOneShot(elevatorArrived[Random.Range(0, elevatorArrived.Length)], elevatorArrivedVol);
    }

    
}
