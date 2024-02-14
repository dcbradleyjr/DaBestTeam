using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class elevatorScript : MonoBehaviour
{
    public float speed;
    public float wallSpeed;
    public int startingPoint;
    public Transform[] points;
    public Transform pushWall;
    public Transform endPoint;

    public bool isMoving;


    private int i;

    void Start()
    {
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        if (isMoving)
        {
            pushPlayer();
        }


    }

    public void ElevatorMovement()
    {
        isMoving = true;
        i++;
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void pushPlayer()
    {
        pushWall.position = Vector3.MoveTowards(pushWall.position, endPoint.position, wallSpeed * Time.deltaTime);
    }
}
