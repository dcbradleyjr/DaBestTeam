using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] Elevator LinkedElevator;
    [SerializeField] List<ElevatorManager> ElevatorFloors;

    public Elevator ActiveElevator => LinkedElevator;
    public List<ElevatorManager> Floors => ElevatorFloors;

    public void CallElevator(ElevatorManager requestedFloor, bool isMoving)
    {
        Debug.Log("Step 3");
        // already at this floor
        /*if (requestedFloor == ActiveElevator.CurrentFloor)
            return;*/ //removed for ability to move elevator
        
        LinkedElevator.MoveTo(requestedFloor);
    }

    public void SendElevatorTo(ElevatorManager requestedFloor)
    {
        Debug.Log("Here we go!");
        // already at this floor
        if (requestedFloor == ActiveElevator.CurrentFloor)
            return;

        LinkedElevator.MoveTo(requestedFloor);
    }
}
