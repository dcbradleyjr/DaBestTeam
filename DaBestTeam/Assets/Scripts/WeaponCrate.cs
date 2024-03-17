using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCrate : MonoBehaviour, IInteract
{
    [SerializeField] string prompt;
    [SerializeField] int gunIndex;
    [SerializeField] int cost;

    [Header("---Melee---")]
    [Header("0 = Stick")]
    [Header("1 = Bat")]
    [Header("2 = CrowBar")]
    [Header("3 = Pan")]
    [Header("4 = Axe")]
    [Header("5 = Sword")]
    public bool Melee;

    [Header("---Pistol---")]
    [Header("0 = Pistol")]
    [Header("1 = Revolver")]
    public bool Pistol;

    [Header("---Gun---")]
    [Header("0 = AR")]
    [Header("1 = Shotgun")]
    [Header("2 = Sniper")]
    [Header("3 = SMG")]
    public bool Gun;
    public string interactPrompt => prompt;

    private void Start()
    {
        prompt += " (" + cost.ToString() + ") ";
    }

    public void interact()
    {

        int playerCurrency = gameManager.instance.GetCurrencyBalance();

        if (playerCurrency >= cost)
        {
            gameManager.instance.SpendCurrency(cost);
            if (Melee)
            {
                if (!WeaponSlotManager.instance.canToggleMelee)
                {
                    WeaponSlotManager.instance.UnlockMeleeSlot();
                }
                WeaponSlotManager.instance.Melee.ToggleMelee(gunIndex);
                WeaponSlotManager.instance.ToggleMeleeSlot();
            }

            if (Gun)
            {
                if (!WeaponSlotManager.instance.canToggleGun)
                {
                    WeaponSlotManager.instance.UnlockGunSlot();
                }
                WeaponSlotManager.instance.Gun.ToggleGun(gunIndex);
                WeaponSlotManager.instance.ToggleGunSlot();
            }
            if (Pistol)
            {
                if (!WeaponSlotManager.instance.canTogglePistol)
                {
                    WeaponSlotManager.instance.UnlockPistolSlot();
                }
                WeaponSlotManager.instance.Pistol.ToggleGun(gunIndex);
                WeaponSlotManager.instance.TogglePistolSlot();
            }
        }
        else
        {
            Debug.Log("You're broke");
        }
    }
}
