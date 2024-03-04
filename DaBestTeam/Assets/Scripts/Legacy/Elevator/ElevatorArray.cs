using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElevatorArray : MonoBehaviour, IInteract
{
    [SerializeField] TextMeshProUGUI ButtonText;
    public ElevatorExit[] elevatorManager;

    public Elevator elevator;

    

    public void interact()
    {
        Debug.Log("Step 1");
        if (!gameManager.instance.canProgress)
            return;
        for (int i = 0; i < elevatorManager.Length - 1; i++)//checking what floor except for last one to stop blast off
        {
            if (elevatorManager[i].DisplayName == elevator.CurrentFloor.DisplayName && i + 1 <= gameManager.instance.currentLevel + 1)//compares to display name to move up one floor
            {
                elevatorManager[i+1].OnCallElevator();
                return;//stops with wasting performence
            }
        }
    }
}
