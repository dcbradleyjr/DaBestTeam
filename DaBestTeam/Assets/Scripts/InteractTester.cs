using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTester : MonoBehaviour, IInteract
{
    [SerializeField] string prompt;
    public string interactPrompt => prompt;
    
    public void interact()
    {
        Debug.Log(interactPrompt);
    }
}
