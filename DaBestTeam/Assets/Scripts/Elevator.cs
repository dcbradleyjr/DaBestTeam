using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] ElevatorController LinkedController;
    [SerializeField] ElevatorFloor StartingFloor;
    [SerializeField] float ElevatorSpeed = 2f;
    [SerializeField] Transform ElevatorUIRoot;
    [SerializeField] GameObject ElevatorUIButtonPrefab;

    Animator LinkedAnimator;

    public ElevatorFloor CurrentFloor { get; private set; } = null;
    public ElevatorFloor TargetFloor { get; private set; } = null;
    public bool IsMoving { get; private set; } = false;

    private void Awake()
    {
        LinkedAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, StartingFloor.TargetY, transform.position.z);
        CurrentFloor = StartingFloor;

        //add the floor UI
        foreach (var floor in LinkedController.Floors)
        {
            var elevatorUIGO = Instantiate(ElevatorUIButtonPrefab, ElevatorUIRoot);
            elevatorUIGO.GetComponent<ElevatorUI_FloorButton>().Bind(floor, LinkedController, floor.DisplayName);
        }
    }

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

    public void MoveTo(ElevatorFloor targetFloor)
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
}
