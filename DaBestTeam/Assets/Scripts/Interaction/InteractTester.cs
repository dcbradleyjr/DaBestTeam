using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTester : MonoBehaviour, IInteract
{
    [SerializeField] string prompt;
    [SerializeField] bool test;
    public string interactPrompt => prompt;
    
    public void interact()
    {
        Debug.Log("Cube Opened!");
        WeaponSlotManager.instance.FreezeWeapons(test);
    }
}
