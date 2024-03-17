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
            WeaponSlotManager.instance.ActivateMeleeSlot();
            WeaponSlotManager.instance.Melee.ActivateMelee(0);
            gameObject.SetActive(false);
        }
        if (isRangedPickUp)
        {
            WeaponSlotManager.instance.ActivatePistolSlot();
            WeaponSlotManager.instance.Pistol.ActivateGun(0);
            gameObject.SetActive(false);
        }
    }
}
