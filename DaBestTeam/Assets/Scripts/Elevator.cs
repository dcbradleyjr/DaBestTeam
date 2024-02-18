using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] ElevatorController LinkedController;
    [SerializeField] ElevatorManager StartingFloor;
    [SerializeField] float ElevatorSpeed = 2f;
    [SerializeField] Transform ElevatorUIRoot;
    [SerializeField] GameObject ElevatorUIButtonPrefab;

    Animator LinkedAnimator;

    public bool isMoving;

    public ElevatorManager CurrentFloor { get; private set; } = null;
    public ElevatorManager TargetFloor { get; private set; } = null;
    public bool IsMoving { get; private set; } = false;

    private Transform currentFloor;//Temp
    public Transform end;//Temp

    private void Awake()
    {
        LinkedAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, StartingFloor.TargetY, transform.position.z);
        CurrentFloor = StartingFloor;

        //add the floor buttons UI
        //FloorButtonCreator();
    }

    /*private void FloorButtonCreator()
    {
        foreach (var floor in LinkedController.Floors)
        {
            var elevatorUIGO = Instantiate(ElevatorUIButtonPrefab, ElevatorUIRoot);
            elevatorUIGO.GetComponent<elevatorButtons>().Bind(floor, LinkedController, floor.DisplayName);
        }
    }*/

    // Update is called once per frame
    void FixedUpdate()
    {       
        if (IsMoving)
        {
            Vector3 targetLocation = transform.position;
            targetLocation.y = TargetFloor.TargetY;

            transform.position = Vector3.MoveTowards(transform.position, targetLocation, ElevatorSpeed * Time.deltaTime);

            //When we arrive
            if (Vector3.Distance(transform.position, targetLocation) < float.Epsilon)
            {
                IsMoving = false;
                CurrentFloor = TargetFloor;
                TargetFloor = null;

                CurrentFloor.OnElevatorArrived(this);
            }
        }
    }
    //Tells it is moving and departed
    public void MoveTo(ElevatorManager targetFloor)
    {
        IsMoving = true;
        TargetFloor = targetFloor;
        CurrentFloor.OnElevatorDeparted(this);
    }

    public void OpenDoors()
    {
        LinkedAnimator.ResetTrigger("Close");
        LinkedAnimator.SetTrigger("Open");
    }

    public void CloseDoors()
    {
        LinkedAnimator.ResetTrigger("Open");
        LinkedAnimator.SetTrigger("Close");
    }

    public void ElevatorMovement()
    {
        isMoving = true;
    }//Temp
}
