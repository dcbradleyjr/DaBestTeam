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
    [SerializeField] public SwitchVCam switchCam;
    [SerializeField] PlayerInput input;
    [SerializeField] GameObject hitMarker;

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

    public bool isMeleeActive;
    public bool isPistolActive;
    public bool isGunActive;

    void Awake()
    {
        instance = this;
        meleeSwitch = input.actions["MeleeSelect"];
        pistolSwitch = input.actions["PistolSelect"];
        gunSwitch = input.actions["GunSelect"];
        Melee = MeleeSlot.GetComponent<MeleeSlot>();
        Pistol = PistolSlot.GetComponent<GunSlot>();
        Gun = GunSlot.GetComponent<GunSlot>();
        switchCam = GameObject.FindWithTag("AimCinemachine").GetComponent<SwitchVCam>();
    }

    private void Update()
    {
        if (meleeSwitch.triggered && canToggleMelee && !IsReloadingGuns())
            ToggleMeleeSlot();
        if (pistolSwitch.triggered & canTogglePistol && !IsReloadingGuns())
            TogglePistolSlot();
        if (gunSwitch.triggered && canToggleGun && !IsReloadingGuns())
            ToggleGunSlot();
    }

    public void ToggleMeleeSlot()
    {
        PistolSlot.SetActive(false);
        GunSlot.SetActive(false);
        MeleeSlot.SetActive(true);
        switchCam.MeleeReticle();
        if (!isMeleeActive)
            AudioManager.instance.PlaySFX("ToggleMelee");

        isMeleeActive = true;
        isPistolActive = false;
        isGunActive = false;
        UIManager.instance.ammoCurrentDisplay.text = "";
        UIManager.instance.weaponNameDisplay.text = Melee.GetName();
    }

    public void TogglePistolSlot()
    {
        MeleeSlot.SetActive(false);
        GunSlot.SetActive(false);
        PistolSlot.SetActive(true);
        switchCam.PistolReticle();
        if (!isPistolActive)
            AudioManager.instance.PlaySFX("TogglePistol");

        UIManager.instance.AmmoDisplay.SetActive(true);
        isPistolActive = true;
        isMeleeActive = false;
        isGunActive = false;
        Pistol.UpdateUI();
    }
    public void ToggleGunSlot()
    {
        MeleeSlot.SetActive(false);
        PistolSlot.SetActive(false);
        GunSlot.SetActive(true);
        switchCam.GunReticle();
        if (!isGunActive)
            AudioManager.instance.PlaySFX("ToggleGun");

        UIManager.instance.AmmoDisplay.SetActive(true);
        isGunActive = true;
        isPistolActive = false;
        isMeleeActive = false;
        Gun.UpdateUI();
    }

    public void DeactivateSlots()
    {
        MeleeSlot.SetActive(false);
        PistolSlot.SetActive(false);
        GunSlot.SetActive(false);
        isMeleeActive = false;
        isPistolActive = false;
        isGunActive = false;
        switchCam.MeleeReticle();
        UIManager.instance.ammoCurrentDisplay.text = "";
    }

    public void LockMeleeSlot()
    {
        canToggleMelee = false;
        MeleeSlot.SetActive(false);
        isMeleeActive = false;
    }

    public void LockPistolSlot()
    {
        canTogglePistol = false;
        PistolSlot.SetActive(false);
        isPistolActive = false;
    }

    public void LockGunSlot()
    {
        canToggleGun = false;
        GunSlot.SetActive(false);
        isGunActive = false;
    }

    public void UnlockMeleeSlot()
    {
        canToggleMelee = true;
    }
    public void UnlockPistolSlot()
    {
        canTogglePistol = true;
    }

    public void UnlockGunSlot()
    {
        canToggleGun = true;
    }

    public void ClearEquippedWeapons()
    {
        Melee.DisableAllMelee();
        Pistol.DisableAllGuns();
        Gun.DisableAllGuns();
    }

    public bool IsReloadingGuns()
    {
        return Gun.isReloading || Pistol.isReloading;
    }

    public void SaveWeapons()
    {
        PlayerPrefs.SetInt("CurrentMeleeIndex", Melee.GetWeaponIndex());
        PlayerPrefs.SetInt("CurrentPistolIndex", Pistol.GetWeaponIndex());
        PlayerPrefs.SetInt("CurrentGunIndex", Gun.GetWeaponIndex());

        PlayerPrefs.SetInt("CanToggleMelee", canToggleMelee ? 1 : 0);
        PlayerPrefs.SetInt("CanTogglePistol", canTogglePistol ? 1 : 0);
        PlayerPrefs.SetInt("CanToggleGun", canToggleGun ? 1 : 0);

        PlayerPrefs.SetInt("isMelee", isMeleeActive ? 1 : 0);
        PlayerPrefs.SetInt("isPistol", isPistolActive ? 1 : 0);
        PlayerPrefs.SetInt("isGun", isGunActive ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void LoadWeapons()
    {
        int currentMeleeIndex = PlayerPrefs.GetInt("CurrentMeleeIndex", -1);
        int currentPistolIndex = PlayerPrefs.GetInt("CurrentPistolIndex", -1);
        int currentGunIndex = PlayerPrefs.GetInt("CurrentGunIndex", -1);

        canToggleMelee = PlayerPrefs.GetInt("CanToggleMelee", 0) == 1 ? true : false;
        canTogglePistol = PlayerPrefs.GetInt("CanTogglePistol", 0) == 1 ? true : false;
        canToggleGun = PlayerPrefs.GetInt("CanToggleGun", 0) == 1 ? true : false;

        ClearEquippedWeapons();
        DeactivateSlots();

        if (currentMeleeIndex != -1 && canToggleMelee)
        {
            UnlockMeleeSlot();
            Melee.ToggleMelee(currentMeleeIndex);
        }

        if (currentPistolIndex != -1 && canTogglePistol)
        {
            UnlockPistolSlot();
            Pistol.ToggleGun(currentPistolIndex);
        }

        if (currentGunIndex != -1 && canToggleGun)
        {
            UnlockGunSlot();
            Gun.ToggleGun(currentGunIndex);
        }

        if (PlayerPrefs.GetInt("isMelee", 0) == 1 ? true : false) ToggleMeleeSlot();
        if (PlayerPrefs.GetInt("isPistol", 0) == 1 ? true : false) TogglePistolSlot();
        if (PlayerPrefs.GetInt("isGun", 0) == 1 ? true : false) ToggleGunSlot();
    }

    public void ResetWeapons()
    {
        Melee.SetWeaponIndex(-1);
        Pistol.SetWeaponIndex(-1);
        Gun.SetWeaponIndex(-1);

        DeactivateSlots();
        ClearEquippedWeapons();
    }

    public void StartHitmarker()
    {
        StartCoroutine(HitMarkerToggle());
    }

    IEnumerator HitMarkerToggle()
    {
        hitMarker.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitMarker.SetActive(false);
    }

    public void FreezeWeapons(bool force)
    {
        if (force)
        {
            Melee.freezeWeapon = true;
            Pistol.freezeWeapon = true;
            Gun.freezeWeapon = true;
            switchCam.aimCanvas.enabled = false;
            switchCam.thirdPersonCanvas.enabled = false;
        }
        else
        {
            Melee.freezeWeapon = false;
            Pistol.freezeWeapon = false;
            Gun.freezeWeapon = false;
            switchCam.aimCanvas.enabled = true;
            switchCam.thirdPersonCanvas.enabled = true;
        }
    }

}
