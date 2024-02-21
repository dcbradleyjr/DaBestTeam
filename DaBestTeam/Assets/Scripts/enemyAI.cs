using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Animator animator;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPosition;
    [SerializeField] Transform headPosition;

    [SerializeField] int HP;
    [SerializeField] int viewCone;
    [SerializeField] int shootCone;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDistance;
    [SerializeField] int animSpeedTrans;


    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    [SerializeField] bool canPatrol;
    [SerializeField] int waitTimeAtDest;
    public Transform[] waypoints;
    
    public UnityEngine.UI.Image HealthBar;
    public GameObject EnemyUI;

    bool isShooting;
    bool playerInRange;
    bool destinationChosen;
    float angleToPlayer;
    float stoppingDistanceOrig;
    Vector3 playerDirection;
    Vector3 startingPosition;
    int HPOriginal;

    void Start()
    {
        gameManager.instance.updateEnemyCount(1);
        HPOriginal = HP;
        startingPosition = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
        updateUI();
    }

    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));
        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
       else if (!playerInRange)
        {
            StartCoroutine(roam());
        }
    }

    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
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
                if (!isShooting && angleToPlayer <= shootCone)
                {
                    StartCoroutine(shoot());
                }
                if (agent.remainingDistance < agent.stoppingDistance)
                    faceTarget();

                agent.stoppingDistance = stoppingDistanceOrig;
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
        agent.stoppingDistance = 0;
    }

    public void takeDamage(int amount)
    {
        agent.SetDestination(gameManager.instance.player.transform.position);

        HP -= amount;
        updateUI();
        if(!EnemyUI.gameObject.activeSelf)
            EnemyUI.gameObject.SetActive(true);

        StartCoroutine(flashMat());
        if (HP <= 0)
        {
            Destroy(gameObject);
            gameManager.instance.updateEnemyCount(-1);
        }
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
