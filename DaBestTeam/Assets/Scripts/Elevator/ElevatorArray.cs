using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElevatorArray : MonoBehaviour, IInteract
{
    [SerializeField] TextMeshProUGUI ButtonText;

    public ElevatorManager[] elevatorManager;

    public Elevator elevator;

    public void interact()
    {
        if (!gameManager.instance.canProgress)
            return;
        for (int i = 0; i < elevatorManager.Length - 1; i++)//checking what floor except for last one to stop blast off
        {
            if (elevatorManager[i].DisplayName == elevator.CurrentFloor.DisplayName)//compares to display name to move up one floor
            {
                elevatorManager[i+1].OnCallElevator();
                return;//stops with wasting performence
            }
        }
    }
}