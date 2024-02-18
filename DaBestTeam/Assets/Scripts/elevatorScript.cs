using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class elevatorScript : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform start;
    public Transform end;
    public Transform endPoint;

    public bool isMoving;


    private Transform currentFloor;

    void Start()
    {
        currentFloor = start;
        transform.position = currentFloor.position;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ElevatorMovement()
    {
        isMoving = true;
        currentFloor = end;
    }

}
