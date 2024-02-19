using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class elevatorButtons : MonoBehaviour, IInteract
{
    [SerializeField] TextMeshProUGUI ButtonText;

    public ElevatorManager elevatorManager;

    

    public void interact()
    {
        Debug.Log("Step 1");
        elevatorManager.OnCallElevator();
    }
}
