using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class elevatorScript : MonoBehaviour
{
    public float speed;
    public float wallSpeed;
    public int startingPoint;
    public Transform start;
    public Transform end;
    public Transform pushWall;
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
        transform.position = Vector3.MoveTowards(transform.position, currentFloor.position, speed * Time.deltaTime);
        if (isMoving)
        {
            pushPlayer();
        }
    }

    public void ElevatorMovement()
    {
        isMoving = true;
        currentFloor = end;
    }

    public void pushPlayer()
    {
        pushWall.position = Vector3.MoveTowards(pushWall.position, endPoint.position, wallSpeed * Time.deltaTime);
    }
}
