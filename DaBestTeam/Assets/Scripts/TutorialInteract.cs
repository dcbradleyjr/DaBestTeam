using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteract : MonoBehaviour, IInteract
{
    [SerializeField] string prompt;

    public bool isMeleePickUp;
    public bool isRangedPickUp;

    public string interactPrompt => prompt;

    // Update is called once per frame
    void Start()
    {
        if (this.name == "Melee Pickup")
            isMeleePickUp = true;     
        if (this.name == "Ranged Pickup")
            isRangedPickUp = true;
    }

    public void interact()
    {
        if (isMeleePickUp)
        {
            WeaponSlotManager.instance.Melee.ToggleMelee(0);
            WeaponSlotManager.instance.ToggleMeleeSlot();
            WeaponSlotManager.instance.UnlockMeleeSlot();
            gameObject.SetActive(false);
        }
        if (isRangedPickUp)
        {
            WeaponSlotManager.instance.Pistol.ToggleGun(0);
            WeaponSlotManager.instance.TogglePistolSlot();
            WeaponSlotManager.instance.UnlockPistolSlot();
            gameObject.SetActive(false);
        }
    }
}
