using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

/*Idle,
Roam,
ChasePlayer,
Attack,
Death*/


public class ZombieAI : MonoBehaviour, IDamage, IPushBack
{

    [SerializeField] Transform headPosition;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject[] Ragdoll;
    [SerializeField] GameObject RagDollBody;
    [SerializeField] float dropRate;


    [SerializeField] Animator anim;
    [SerializeField] GameObject[] meshMilitaryOptions;
    [SerializeField] GameObject[] meshZombie;
    [SerializeField] GameObject[] zombieWeapon;

    [SerializeField] int damageAmount;

    public bool canHoldWeapons;
    public bool randomMesh;

    [Header("--Stats--")]
    [Range(1, 50000)][SerializeField] int HP;
    [Range(1, 8)][SerializeField] int roamPauseTime;
    [Range(0, 45)][SerializeField] int roamDistance;
    [Range(30, 180)][SerializeField] int viewCone;
    [Range(1, 10)][SerializeField] int targetFaceSpeed;
    [SerializeField] float attackCooldown;


    [Header("--UI--")]
    public UnityEngine.UI.Image HealthBar;
    public GameObject EnemyUI;


    [SerializeField] private AIStateId _state = AIStateId.Roam;

    int HPOriginal;
    float angleToPlayer;
    Vector3 playerDirection;
    Vector3 startingPosition;
    float stoppingDistanceOrig;
    bool destChosen;
    bool playerInRange;
    float attackRange = 2f;

    public enum AIStateId { Roam = 1, ChasePlayer = 2, Attack = 3, Death = 4 };

    public void Start()
    {
        state = AIStateId.Roam;
        HPOriginal = HP;
        startingPosition = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
        updateUI();
        chooseRandomMesh();
        chooseWeapon();
    }

    public void Update()
    {
        takeDamage(1);
    }
    public AIStateId state
        {
        get {return _state;}

        set {

            StopAllCoroutines();
            _state = value;

            switch (state)
                {
                case AIStateId.Roam:
                        StartCoroutine(roamState());
                    break;
                case AIStateId.ChasePlayer:
                        StartCoroutine(chaseState());
                    break;
                case AIStateId.Attack:
                        StartCoroutine(attackState());
                    break;
                case AIStateId.Death:
                        StartCoroutine (deathState());
                    break;

                }

            }
        }
    public IEnumerator roamState()
    {
        while (true)
        {
            
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                Debug.Log("Roaming");
                Vector3 randomPos = Random.insideUnitSphere * roamDistance;
                randomPos += startingPosition;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomPos, out hit, roamDistance, 1);
                agent.SetDestination(hit.position);
                
            }

            yield return new WaitForSeconds(roamPauseTime);
        }

    }
    public IEnumerator chaseState()
    {
        while (true)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);
            if (distanceToPlayer < attackRange)
            {
                Debug.Log("Log");
                // Transition to attack state
                StartCoroutine(attackState());
                // Exit chase state
                yield break;
            }
            // Chase the player
            agent.SetDestination(gameManager.instance.player.transform.position);

            yield return null;
        }
    }
    public IEnumerator attackState()
    {
        Debug.Log("Pow");
        while (true)
        {
            
            gameManager.instance.takeDamage(damageAmount); //Update when player is complete

            // Check if the player is still in range
            float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);
            if (distanceToPlayer > attackRange)
            {
                state = AIStateId.ChasePlayer;
                yield break;
            }

            // Attack cooldown
            yield return new WaitForSeconds(attackCooldown);
        }
    }
    public IEnumerator deathState()
    {
        
            bool dropItem = Random.value <= dropRate;// Drop rate set on SerializedField .01 = 1%

            if (dropItem)
            {
                Instantiate(RagDollBody, transform.position, Quaternion.identity);
            }


            int randomIndex = Random.Range(0, Ragdoll.Length);
            Instantiate(Ragdoll[randomIndex],transform.position, Quaternion.identity);
            Destroy(gameObject);
            yield return null;
        
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


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            state = AIStateId.ChasePlayer;
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

        state = AIStateId.ChasePlayer;

        HP -= amount;
        updateUI();
        if (!EnemyUI.gameObject.activeSelf)
            EnemyUI.gameObject.SetActive(true);

        if (HP <= 0)
        {
            state = AIStateId.Death;
        }
    }

    public void pushBackDir(Vector3 dir)
    {
        agent.velocity += (dir / 2);
    }

    public void chooseRandomMesh()
    {
        if (randomMesh && meshZombie.Length > 0)
        {
            int randomIndex = Random.Range(0, meshZombie.Length);
            meshZombie[randomIndex].gameObject.SetActive(true);
        }
        if (canHoldWeapons && !randomMesh)
        {
            int randomIndex = Random.Range(0, meshMilitaryOptions.Length);
            meshMilitaryOptions[randomIndex].gameObject.SetActive(true);
        }
                
    }
    public void chooseWeapon()
    {
        if (canHoldWeapons)
        {
            int randomIndex = Random.Range(0, zombieWeapon.Length);
            zombieWeapon[randomIndex].gameObject.SetActive(true);
        }
    }
}

