using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] Elevator LinkedElevator;
    [SerializeField] List<ElevatorExit> ElevatorFloors;

    public Elevator ActiveElevator => LinkedElevator;
    public List<ElevatorExit> Floors => ElevatorFloors;

    [SerializeField] Transform parentTransform;

    public void CallElevator(ElevatorExit requestedFloor, bool isMoving)
    {

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerObject.transform.parent = parentTransform;

        Debug.Log("Step 3");
        LinkedElevator.MoveTo(requestedFloor);
    }

    public void SendElevatorTo(ElevatorExit requestedFloor)
    {
        Debug.Log("Here we go!");
        // already at this floor
        if (requestedFloor == ActiveElevator.CurrentFloor)
            return;

        LinkedElevator.MoveTo(requestedFloor);
    }
}
