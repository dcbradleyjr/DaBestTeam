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
            WeaponSlotManager.instance.canToggleMelee = true;
            if(WeaponSlotManager.instance.PistolSlot)
                WeaponSlotManager.instance.PistolSlot.SetActive(false);
            WeaponSlotManager.instance.MeleeSlot.SetActive(true);
            gameObject.SetActive(false);
        }
        if (isRangedPickUp)
        {
            WeaponSlotManager.instance.canTogglePistol = true;
            if (WeaponSlotManager.instance.MeleeSlot)
                WeaponSlotManager.instance.MeleeSlot.SetActive(false);
            WeaponSlotManager.instance.PistolSlot.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
