using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ElevatorExit : MonoBehaviour
{
    
    [SerializeField] string _DisplayName;
    [SerializeField] string SupportedTag = "Player";
    [SerializeField] ElevatorController LinkedController;
    [SerializeField] Transform ElevatorTarget;

    
    Animator LinkedAnimator;

    public string DisplayName => _DisplayName;
    public float TargetY => ElevatorTarget.position.y;
    [SerializeField] bool ElevatorPresent => LinkedController.ActiveElevator.CurrentFloor == this;

    private void Awake()
    {
        LinkedAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (gameManager.instance.canProgress && !LinkedController.ActiveElevator.isMoving)
        {
            LinkedAnimator.ResetTrigger("Close");
            LinkedAnimator.SetTrigger("Open");
            LinkedController.ActiveElevator.OpenDoors();
        }
        else
        {
            LinkedAnimator.ResetTrigger("Open");
            LinkedAnimator.SetTrigger("Close");
            LinkedController.ActiveElevator.CloseDoors();
        }
    }

    //Calls elevator to location
    public void OnCallElevator()
    {
        
        Debug.Log("Step 2");
        LinkedController.CallElevator(this, true);
        LinkedController.ActiveElevator.isMoving = true;
    }
    //Departed calls animator
    public void OnElevatorDeparted(Elevator activeElevator)
    {
        Debug.Log("Step 5");
        LinkedAnimator.ResetTrigger("Open");
        LinkedAnimator.SetTrigger("Close");
        LinkedController.ActiveElevator.CloseDoors();
    }
    //Arrived calls animator
    public void OnElevatorArrived(Elevator activeElevator)
    {
            Debug.Log("Step 6");

            AudioManager.instance.elevatorArrivedSound();
        LinkedController.ActiveElevator.isMoving = false;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            playerObject.transform.parent = null;

            LinkedAnimator.ResetTrigger("Close");
            LinkedAnimator.SetTrigger("Open");
            LinkedController.ActiveElevator.OpenDoors();
    }
}
