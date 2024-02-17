using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevatorButtons : MonoBehaviour, IInteract
{

    public elevatorScript elevator;

    public void interact()
    {
        if (gameManager.instance.canProgress)
        {
            elevator.ElevatorMovement();
            elevator.isMoving = true;
        }
    }
 }
