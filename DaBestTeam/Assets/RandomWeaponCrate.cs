using System.Collections;
using System.Collections.Generic;
using Unity.Profiling.LowLevel;
using UnityEngine;

public class RandomWeaponCrate : MonoBehaviour, IInteract
{

    [SerializeField] string prompt;
    public string interactPrompt => prompt;


    [SerializeField] Animator lidController;
    [SerializeField] Animation gunMovement;
    [SerializeField] GameObject[] weapons;
    [SerializeField] GameObject crateLight;
    [SerializeField] int selectedWeapon;
    [SerializeField] Transform movement;
    [SerializeField] bool openBox;
    [SerializeField] bool purchasableState;
    [SerializeField] int cost = 500;
    bool obtainableState;
    bool startcooldown;
    bool acquiredWeapon;

    float timer;
    int counter, counterCompare;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (openBox)
        {
            OpenRandomCrate();
            openBox = false;
            crateLight.SetActive(true);
            startcooldown = true;
        }
        else if (gunMovement.IsPlaying("GunMovement"))
        {
            timer += Time.deltaTime;
            if (timer < 4.0f && counter < counterCompare)
            {
                counter++;
            }
            else if (counter == counterCompare)
            {
                counter = 0;
                RandomizeWeapon();
                counterCompare++;
                prompt = "Please Wait!";
            }
            if (timer > 4.0f && !acquiredWeapon)
            {
                obtainableState = true;
                prompt = "Pick Up Weapon!";
            }
            weapons[selectedWeapon].transform.position = movement.position;
        }
        else
        {
            if (startcooldown)
            {
                startcooldown = false;
                Debug.Log("Reset");
                counter = 0;
                counterCompare = 0;
                timer = 0;
                obtainableState = false;
                acquiredWeapon = false;
                DisableGuns();
                prompt = "Purchase Random Weapon Cost: 500";
                crateLight.SetActive(false);
                if (!purchasableState)
                    StartCoroutine(Cooldown()); 
            }
        }
    }

    public void interact()
    {
        if (obtainableState)
        {
            if (selectedWeapon >= 0 && selectedWeapon < 4)
            {
                if (!WeaponSlotManager.instance.canToggleGun)
                    WeaponSlotManager.instance.UnlockGunSlot();

                WeaponSlotManager.instance.Gun.ToggleGun(selectedWeapon);
                WeaponSlotManager.instance.ToggleGunSlot();
            }
            else if (selectedWeapon >= 4 && selectedWeapon < 10)
            {
                if (!WeaponSlotManager.instance.canToggleMelee)
                    WeaponSlotManager.instance.UnlockMeleeSlot();

                WeaponSlotManager.instance.Melee.ToggleMelee(selectedWeapon - 4);
                WeaponSlotManager.instance.ToggleMeleeSlot();

            }
            else if (selectedWeapon >= 10 && selectedWeapon < 12)
            {
                if (!WeaponSlotManager.instance.canTogglePistol)
                    WeaponSlotManager.instance.UnlockPistolSlot();

                WeaponSlotManager.instance.Pistol.ToggleGun(selectedWeapon - 10);
                WeaponSlotManager.instance.TogglePistolSlot();
            }
            acquiredWeapon = true;
            obtainableState = false;
            prompt = "Cooling down!";
            crateLight.SetActive(false);
            DisableGuns();
            CloseLid();
        }
        else if (purchasableState && !gunMovement.IsPlaying("GunMovement"))
        {
            int playerCurrency = gameManager.instance.GetCurrencyBalance();

            if (playerCurrency >= cost)
            {
                purchasableState = false;
                gameManager.instance.SpendCurrency(cost);
                openBox = true;
            }
            else
            {
                Debug.Log("You don't have enough currency to open the box.");
            }
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f); // Example cooldown duration, adjust as needed
        Debug.Log("cooldown");
        purchasableState = true; // Player can purchase the box again after cooldown
    }

    void OpenRandomCrate()
    {
        OpenLid();
        GunMovement();
    }

    void OpenLid()
    {
        lidController.Play("OpenLid");
    }

    void CloseLid()
    {
        lidController.Play("CloseLid");

    }

    void GunMovement()
    {
        gunMovement.Play();
    }

    void RandomizeWeapon()
    {
        int weaponCount = weapons.Length;
        int rand = Random.Range(0, weaponCount);

        while (rand == selectedWeapon)
        {
            rand = Random.Range(0, weaponCount);
        }
        selectedWeapon = rand;

        DisableGuns();

        weapons[selectedWeapon].SetActive(true);
        weapons[selectedWeapon].transform.position = movement.position;
    }

    void DisableGuns()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
    }
}
