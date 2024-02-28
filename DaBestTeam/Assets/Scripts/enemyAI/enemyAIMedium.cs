using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class enemyAIMedium : MonoBehaviour, IDamage, IPatrol
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPosition;
    [SerializeField] Transform headPosition;

    [SerializeField] int HP;
    [SerializeField] int viewCone;
    [SerializeField] int targetFaceSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    public Transform[] waypoints;
    int currentWaypointIndex;
    float waitTime = 2.0f;
    float remainingTime;
    bool isPatrolling = true;
    Vector3 posBeforeSeePlayer;

    public UnityEngine.UI.Image HealthBar;
    public GameObject EnemyUI;

    bool isShooting;
    bool playerInRange;
    float angleToPlayer;
    Vector3 playerDirection;
    int HPOriginal;

    void Start()
    {
        gameManager.instance.updateEnemyCount(1);
        HPOriginal = HP;
        updateUI();
        remainingTime = waitTime; //set wait time at each destination to the remaining time value
        agent.SetDestination(waypoints[currentWaypointIndex].position); //will move towards the patrol point if they can patrol

        posBeforeSeePlayer = Vector3.zero;
    }

    void Update()
    {
        if (playerInRange && canSeePlayer())
        {
            isPatrolling = false;
        }
        else
        {
            if (posBeforeSeePlayer != Vector3.zero)
            {
                if (remainingTime <= 0)
                {
                    agent.SetDestination(posBeforeSeePlayer);
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        posBeforeSeePlayer = Vector3.zero;
                        isPatrolling = true;
                        remainingTime = waitTime;
                    }
                }
                else
                {
                    remainingTime -= Time.deltaTime;
                }

            }
            else
            {
                if (isPatrolling)
                    patrol();
            }
        }
    }

    bool canSeePlayer()
    {
        playerDirection = gameManager.instance.playerHead.position - headPosition.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        Debug.DrawRay(headPosition.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(headPosition.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                if (posBeforeSeePlayer == Vector3.zero)
                    posBeforeSeePlayer = agent.transform.position;

                if (agent.remainingDistance < agent.stoppingDistance)
                    faceTarget();
                return true;
            }
        }
        return false;

    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDirection.x, transform.position.y, playerDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void takeDamage(int amount)
    {
        isPatrolling = false;
        agent.SetDestination(gameManager.instance.player.transform.position);

        HP -= amount;
        updateUI();
        if (!EnemyUI.gameObject.activeSelf)
            EnemyUI.gameObject.SetActive(true);

        StartCoroutine(flashMat());
        if (HP <= 0)
        {
            Destroy(gameObject);
            gameManager.instance.updateEnemyCount(-1);
        }
    }

    public void patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (remainingTime <= 0)
            {
                NextPoint();
                remainingTime = waitTime;
            }
            else
            {
                remainingTime -= Time.deltaTime;
            }
        }
        else
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    public void NextPoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    IEnumerator flashMat()
    {
        Color tempColor = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = tempColor;
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPosition.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void updateUI()
    {
        HealthBar.fillAmount = (float)HP / HPOriginal;
    }
}
