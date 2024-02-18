using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] Elevator LinkedElevator;
    [SerializeField] List<ElevatorFloor> ElevatorFloors;

    public Elevator ActiveElevator => LinkedElevator;
    public List<ElevatorFloor> Floors => ElevatorFloors;

    public void CallElevator(ElevatorFloor requestedFloor, bool isUp)
    {
        Debug.Log("Its here");
        // already at this floor
        /*if (requestedFloor == ActiveElevator.CurrentFloor)
            return;*/

        LinkedElevator.MoveTo(requestedFloor);
    }

    public void SendElevatorTo(ElevatorFloor requestedFloor)
    {
        Debug.Log("Its here");
        // already at this floor
        /*if (requestedFloor == ActiveElevator.CurrentFloor)
            return;*/

        LinkedElevator.MoveTo(requestedFloor);
    }
}
