using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevatorButtons : MonoBehaviour, IInteract
{

    public elevatorScript elevator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void interact()
    {
        if (gameManager.instance.canProgress)
        {
            elevator.ElevatorMovement();
            elevator.isMoving = true;
        }
    }
 }
