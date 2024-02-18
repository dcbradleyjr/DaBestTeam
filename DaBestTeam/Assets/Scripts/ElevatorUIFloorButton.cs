using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElevatorUI_FloorButton : MonoBehaviour, IInteract
{
    [SerializeField] TextMeshProUGUI ButtonText;

    ElevatorController LinkedController;
    ElevatorFloor LinkedFloor;

    public Elevator elevator;

    public void Bind(ElevatorFloor linkedFloor, ElevatorController linkedController, string floorName)
    {
        LinkedController = linkedController;
        LinkedFloor = linkedFloor;
        ButtonText.text = floorName;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        onPressed();
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

    public void interact()
    {
        if (gameManager.instance.canProgress)
        {
            elevator.ElevatorMovement();
            elevator.isMoving = true;
        }
    }
}
