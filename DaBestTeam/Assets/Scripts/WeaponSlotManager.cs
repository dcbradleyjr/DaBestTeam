using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSlotManager : MonoBehaviour
{
    public static WeaponSlotManager instance;
    [SerializeField] GameObject MeleeSlot;
    [SerializeField] GameObject PistolSlot;
    [SerializeField] GameObject GunSlot;
    [SerializeField] PlayerInput input;

    InputAction meleeSwitch;
    InputAction pistolSwitch;
    InputAction gunSwitch;

    private PistolSlot Pistol;


    void Awake()
    {
        instance = this;
        meleeSwitch = input.actions["MeleeSelect"];
        pistolSwitch = input.actions["PistolSelect"];
        gunSwitch = input.actions["GunSelect"];
        Pistol = PistolSlot.GetComponent<PistolSlot>();
    }

    private void Update()
    {
        if (meleeSwitch.triggered)
            ActivateMeleeSlot();
        if (pistolSwitch.triggered)
            ActivatePistolSlot();
        if (gunSwitch.triggered)
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
}
