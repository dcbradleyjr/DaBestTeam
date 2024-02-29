using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class enemyAIRoam : MonoBehaviour, IDamage
{
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPosition;
    [SerializeField] Transform headPosition;
    [SerializeField] AudioSource aud;
    [SerializeField] int roamPauseTime;
    [SerializeField] float roamDist;
    [Range(1, 10)][SerializeField] int animSpeedTrans;

    [SerializeField] int HP;
    [SerializeField] int viewCone;
    [SerializeField] int targetFaceSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject leadingBullet;
    [SerializeField] bool canShotLead;
    [SerializeField] float shootRate;
    [SerializeField] float bulletSpeed;
    [Range(0.1f, 0.9f)][SerializeField] float bulletLeadChance;

    float waitTime = 3.5f;
    float remainingTime;
    bool isPatrolling = true;
    Vector3 startPos;
    Vector3 posBeforeSeePlayer;
    Vector3 roamDest;
    bool destChosen;

    public UnityEngine.UI.Image HealthBar;
    public GameObject EnemyUI;

    bool isShooting;
    bool playerInRange;
    float angleToPlayer;
    Vector3 playerDirection;
    int HPOriginal;
    float stoppingDistanceOrig;

    void Start()
    {
        HPOriginal = HP;
        updateUI();
        remainingTime = waitTime; //set wait time at each destination to the remaining time value

        startPos = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
        agent.stoppingDistance = 0;
        posBeforeSeePlayer = Vector3.zero;
    }

    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));
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
                if (isPatrolling)
                    StartCoroutine(roam());
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
                    if (canShotLead)
                    {
                        float doesLead = Random.Range(0f, 1.0f);
                        if (doesLead < bulletLeadChance)
                        {
                            StartCoroutine(shoot());
                        }
                        else
                        {
                            StartCoroutine(leadShoot());
                        }
                    }
                    else
                    {
                        StartCoroutine(shoot());
                    }

                }
                if (posBeforeSeePlayer == Vector3.zero)
                    posBeforeSeePlayer = agent.transform.position;

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
    }

    public void takeDamage(int amount)
    {
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

   
    IEnumerator roam()
    {
        Debug.Log("I am roaming");
        if (agent.remainingDistance < 0.05f && !destChosen)
        {
            destChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            roamDest = hit.position;
            agent.SetDestination(roamDest);

            destChosen = false;
        }
        else
        {
            agent.SetDestination(roamDest);
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

    IEnumerator leadShoot()
    {
        isShooting = true;
        Instantiate(leadingBullet, shootPosition.position, transform.rotation); //make the bullet
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void updateUI()
    {
        HealthBar.fillAmount = (float)HP / HPOriginal;
    }
}
