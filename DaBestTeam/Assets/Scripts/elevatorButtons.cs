using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class elevatorButtons : MonoBehaviour, IInteract
{
    [SerializeField] TextMeshProUGUI ButtonText;

    ElevatorController LinkedController;
    ElevatorFloor LinkedFloor;

    public elevatorScript elevator;

    public void interact()
    {
        if (gameManager.instance.canProgress)
        {
            elevator.ElevatorMovement();
            elevator.isMoving = true;
        }
    }


    public void Bind(ElevatorFloor linkedFloor, ElevatorController linkedController, string floorName)
    {
        LinkedController = linkedController;
        LinkedFloor = linkedFloor;
        ButtonText.text = floorName;
    }

    public void onPressed()
    {
        if (Input.GetButtonDown("Interact"))
        {
            interact();
            Debug.Log("I am pressed");
            LinkedController.SendElevatorTo(LinkedFloor);
        }
    }
}
