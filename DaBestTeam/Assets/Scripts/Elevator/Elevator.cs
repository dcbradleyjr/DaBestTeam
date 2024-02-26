using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] ElevatorController LinkedController;
    [SerializeField] ElevatorExit StartingFloor;
    [SerializeField] float ElevatorSpeed = 2f;

    [SerializeField] Animator LinkedAnimator;

    public bool isMoving;

    public ElevatorExit CurrentFloor { get; private set; } = null;
    public ElevatorExit TargetFloor { get; private set; } = null;
    public bool IsMoving { get; private set; } = false;
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, StartingFloor.TargetY, transform.position.z);
        CurrentFloor = StartingFloor;

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
                Debug.Log("I am not moving!");
                IsMoving = false;
                CurrentFloor = TargetFloor;
                TargetFloor = null;

                CurrentFloor.OnElevatorArrived(this);
            }
        }
    }
    //Tells it is moving and departed
    public void MoveTo(ElevatorExit targetFloor)
    {
        Debug.Log("Step 4");
        IsMoving = true;
        TargetFloor = targetFloor;
        CurrentFloor.OnElevatorDeparted(this);
    }

    public void OpenDoors()
    {
        if (gameManager.instance.canProgress == true || gameManager.instance.canOpen == true)
        {
            Debug.Log("I am trying to open");
            LinkedAnimator.ResetTrigger("Close");
            LinkedAnimator.SetTrigger("Open");
        }
    }

    public void CloseDoors()
    {
        if (gameManager.instance.canProgress == false || gameManager.instance.canOpen == false)
        {
            LinkedAnimator.ResetTrigger("Open");
            LinkedAnimator.SetTrigger("Close"); 
        }
    }

   
}
