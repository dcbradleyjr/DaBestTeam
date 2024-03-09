using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/*Idle,
Roam,
ChasePlayer,
Attack,
Death*/


public class ZombieAI : MonoBehaviour
{

    [SerializeField] Transform headPosition;

    public NavMeshAgent agent;
    private AIStateId currentState;

    public bool canHoldWeapons;
    public bool canRoam;

    

    bool destinationChosen;

    [Header("--Stats--")]
    [Range(1, 50)][SerializeField] int HP;
    [Range(1, 8)][SerializeField] int roamPauseTime;
    [Range(0, 45)][SerializeField] int roamDistance;
    [Range(30, 180)][SerializeField] int viewCone;
    [Range(1, 10)][SerializeField] int targetFaceSpeed;

    [Header("--UI--")]
    public UnityEngine.UI.Image HealthBar;
    public GameObject EnemyUI;


    int HPOriginal;
    float angleToPlayer;
    Vector3 playerDirection;
    Vector3 startingPosition;
    float stoppingDistanceOrig;
    public enum AIStateId {Watch = 1, Roam = 2, ChasePlayer = 3, Attack = 4, Death = 5 };

    public void Start()
    {
        currentState = AIStateId.Watch;
        HPOriginal = HP;
        startingPosition = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
        updateUI();
    }

    public void Update()
    {
        switch (currentState)
        {
            case AIStateId.Watch:
                StartCoroutine(WatchState());
                break;
            case AIStateId.Roam:
                if (!canRoam)
                {
                    StartCoroutine(roamState());
                }
                else
                {
                    StartCoroutine(idleState());
                }

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
    public IEnumerator WatchState()
    {
        playerDirection = gameManager.instance.playerHead.position - headPosition.position;
        Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);
        Debug.DrawRay(headPosition.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(headPosition.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                
                StartCoroutine(attackState());

                if (agent.remainingDistance < agent.stoppingDistance)
                    faceTarget();

                agent.stoppingDistance = stoppingDistanceOrig;
            }
        }
        yield return this;
    }
    public IEnumerator roamState()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            AudioManager.instance.enemyStepSound();
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);

            Vector3 randomPos = Random.insideUnitSphere * roamDistance;
            randomPos += startingPosition;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDistance, 1);
            agent.SetDestination(hit.position);
            destinationChosen = false;
        }
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

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDirection.x, transform.position.y, playerDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }

    void updateUI()
    {
        HealthBar.fillAmount = (float)HP / HPOriginal;
    }
}
