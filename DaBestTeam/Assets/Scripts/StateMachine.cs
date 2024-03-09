using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


    /*Idle,
    Roam,
    ChasePlayer,
    Attack,
    Death*/


public class StateMachine : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    private AIStateId currentState;

    public bool canHoldWeapons;
    public bool canRoam;

    public enum AIStateId {Idle = 0,Watch = 1, Roam = 2, ChasePlayer = 3, Attack = 4, Death = 5};

    public void Start()
    {
        currentState = AIStateId.Idle;

    }

    public void Update()
    {
        switch (currentState)
        {
            case AIStateId.Idle:
                if (!canRoam)
                {
                    StartCoroutine(roamState());
                }
                else
                {
                    StartCoroutine(idleState());
                }
                break;
            case AIStateId.Watch; 
                break;
            case AIStateId.Roam:

                break;
            case AIStateId.ChasePlayer:

                break;
            case AIStateId.Attack:

                break;
            case AIStateId.Death:

                break;
            
        }
    }

    public IEnumerator idleState()
    {
        yield return this;
    }
    public IEnumerator roamState() 
    {
        yield return this;
    }
    public IEnumerator chaseState()
    {
        yield return this;
    }
    public IEnumerator attackState()
    {
        yield return this;
    }
    public IEnumerator deathState()
    {
        yield return this;
    }
}
