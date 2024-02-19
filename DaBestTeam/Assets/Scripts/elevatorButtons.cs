using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class elevatorButtons : MonoBehaviour, IInteract
{
    [SerializeField] TextMeshProUGUI ButtonText;

    public ElevatorManager elevatorManager;

    /*public void Bind(ElevatorManager linkedFloor, ElevatorController linkedController, string floorName)
    {
        LinkedController = linkedController;
        LinkedFloor = linkedFloor;
        ButtonText.text = floorName;
    }*/

    public void interact()
    {
        Debug.Log("Step 1");
        elevatorManager.OnCallElevator();
    }
}
