using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSlotManager : MonoBehaviour
{
    [Header("---Components---")]
    public static WeaponSlotManager instance;
    public GameObject MeleeSlot;
    public GameObject PistolSlot;
    public GameObject GunSlot;
    [SerializeField] PlayerInput input;

    //inputs
    InputAction meleeSwitch;
    InputAction pistolSwitch;
    InputAction gunSwitch;

    [Header("---Weapon Slots---")]
    public MeleeSlot Melee;
    public GunSlot Pistol;
    public GunSlot Gun;

    [Header("---Toggles---")]
    public bool canToggleMelee;
    public bool canTogglePistol;
    public bool canToggleGun;

    void Awake()
    {
        instance = this;
        meleeSwitch = input.actions["MeleeSelect"];
        pistolSwitch = input.actions["PistolSelect"];
        gunSwitch = input.actions["GunSelect"];
        Melee = MeleeSlot.GetComponent<MeleeSlot>();
        Pistol = PistolSlot.GetComponent<GunSlot>();
        Gun = GunSlot.GetComponent<GunSlot>();
    }

    private void Update()
    {
        if (meleeSwitch.triggered && canToggleMelee)
            ActivateMeleeSlot();
        if (pistolSwitch.triggered & canTogglePistol)
            ActivatePistolSlot();
        if (gunSwitch.triggered && canToggleGun)
            ActivateGunSlot();
    }

    public void ActivateMeleeSlot()
    {
        PistolSlot.SetActive(false);
        GunSlot.SetActive(false);
        MeleeSlot.SetActive(true);
    }

    public void ActivatePistolSlot()
    {
        MeleeSlot.SetActive(false);
        GunSlot.SetActive(false);
        PistolSlot.SetActive(true);
    }
    public void ActivateGunSlot()
    {
        MeleeSlot.SetActive(false);
        PistolSlot.SetActive(false);
        GunSlot.SetActive(true);
    }

    public void DeactivateSlots()
    {
        MeleeSlot.SetActive(false);
        PistolSlot.SetActive(false);
        GunSlot.SetActive(false);
    }

    public void LockMeleeSlot()
    {
        canToggleMelee = false;
        MeleeSlot.SetActive(false);
    }

    public void LockPistolSlot()
    {
        canTogglePistol = false;
        PistolSlot.SetActive(false);
    }

    public void LockGunSlot()
    {
        canToggleGun = false;
        GunSlot.SetActive(false);
    }
}
