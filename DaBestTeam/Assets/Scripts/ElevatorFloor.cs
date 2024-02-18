using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ElevatorFloor : MonoBehaviour
{
    [SerializeField] string _DisplayName;
    [SerializeField] string SupportedTag = "Player";
    [SerializeField] ElevatorController LinkedController;
    [SerializeField] Transform ElevatorTarget;

    
    Animator LinkedAnimator;

    public string DisplayName => _DisplayName;
    public float TargetY => ElevatorTarget.position.y;
    [SerializeField] bool ElevatorPresent => LinkedController.ActiveElevator.CurrentFloor == this;

    List<GameObject> Openers = new List<GameObject>();

    private void Awake()
    {
        LinkedAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(SupportedTag))
        {
            Openers.Add(other.gameObject);

            if (ElevatorPresent)
            {
                LinkedAnimator.ResetTrigger("Close");
                LinkedAnimator.SetTrigger("Open");
                //LinkedController.ActiveElevator.OpenDoors();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(SupportedTag))
        {
            Openers.Remove(other.gameObject);

            LinkedAnimator.ResetTrigger("Open");
            LinkedAnimator.SetTrigger("Close");
            LinkedController.ActiveElevator.CloseDoors();
        }
    }

    public void OnCallElevatorForUp()
    {
        LinkedController.CallElevator(this, true);
    }

    public void OnCallElevatorForDown()
    {
        LinkedController.CallElevator(this, false);
    }

    public void OnElevatorDeparted(Elevator activeElevator)
    {
        LinkedAnimator.ResetTrigger("Open");
        LinkedAnimator.SetTrigger("Close");
        LinkedController.ActiveElevator.CloseDoors();
    }

    public void OnElevatorArrived(Elevator activeElevator)
    {
        if (Openers.Count > 0)
        {
            LinkedAnimator.ResetTrigger("Close");
            LinkedAnimator.SetTrigger("Open");
            LinkedController.ActiveElevator.OpenDoors();
        }
    }
}
