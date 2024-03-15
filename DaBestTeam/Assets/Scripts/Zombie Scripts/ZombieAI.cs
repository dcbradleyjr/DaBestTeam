using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] Rigidbody[] _ragdollRigidbodies;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;


    [Header("--Components--")]
    [SerializeField] Transform headPosition;    
    [SerializeField] GameObject[] meshMilitaryOptions;
    [SerializeField] GameObject[] meshZombie;
    [SerializeField] GameObject[] zombieWeapon;
    public bool canHoldWeapons;
    public bool randomMesh;
    [SerializeField] Transform HitPoint;
    [SerializeField] float meleeRange;
    [SerializeField] GameObject zombieScratch;

    [Header("--Stats--")]
    [Range(1, 50000)][SerializeField] int HP;
    [Range(1, 8)][SerializeField] int roamPauseTime;
    [Range(0, 45)][SerializeField] int roamDistance;
    [Range(30, 180)][SerializeField] int viewCone;
    [Range(1, 10)][SerializeField] int targetFaceSpeed;
    [SerializeField] float attackCooldown;
    [SerializeField] float dropRate;
    [SerializeField] int damageAmount;
    [SerializeField] float attackAnimSpeed;

    [Header("--UI--")]
    public UnityEngine.UI.Image HealthBar;
    public GameObject EnemyUI;

    public GameObject parentSpawner; //needed for SpawnManager

    [SerializeField] private AIStateId _state = AIStateId.Roam;

    int HPOriginal;
    float angleToPlayer;
    /*Vector3 playerDirection;*/
    Vector3 startingPosition;
    float stoppingDistanceOrig;
    bool destChosen;
    private bool playerInRange;
    float attackRange = 1.5f;
    int attackAnim;
    int walkAnim;
    bool animOrignal;



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
        DisableRagdoll();

        animOrignal = anim;

        attackAnim = Animator.StringToHash("Zombie@Attack01");

        walkAnim = Animator.StringToHash("Zombie@Walk01");

        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();        
    }

    public void Update()
    {
        /*if(state != AIStateId.Death)
        takeDamage(1);*/

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

    private bool isDead;
    private bool isAttacking;

    public IEnumerator roamState()
    {
        while (true)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
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
                // Transition to attack state
                StartCoroutine(attackState());
                // Exit chase state
                yield break;
            }

            int AnimSpeed = Random.Range(0, 1);

            anim.CrossFade(walkAnim, AnimSpeed);
            // Chase the player
            agent.SetDestination(gameManager.instance.player.transform.position);

            yield return null;
        }
    }
    public IEnumerator attackState()
    {
        while (true)
        {

            if (!isAttacking)
            {
                isAttacking = true;
                GameObject scratch = Instantiate(zombieScratch, HitPoint.position, HitPoint.rotation);
                scratch.GetComponent<ZombieScratch>().damage = damageAmount;

                int attackAnimSpeed = Random.Range(0, 1);

                anim.CrossFade(attackAnim, attackAnimSpeed); 
                isAttacking = false;
            }

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
        if (!isDead)
        {
            isDead = true;
            gameManager.instance.EarnCurrency(10);

            this.EnemyUI.SetActive(false);

            this.GetComponent<Animator>().enabled = false;

            this.GetComponent<NavMeshAgent>().enabled = false;

            this.GetComponent<Collider>().enabled = false;

            EnableRagdoll();

            bool dropItem = Random.value <= dropRate;// Drop rate set on SerializedField .01 = 1%

            if (dropItem)
            {
                //Instantiate(RagDollBody, transform.position, Quaternion.identity);
            }

            if (SpawnManager.instance != null)
            {
                SpawnManager.instance.DecrementSpawnTotal(this.gameObject); //spawnManager handles things while this dies
            }


            Destroy(gameObject, 2f);
            yield return null;
        }
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

            if (state  != AIStateId.Death)
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
        HP -= amount;
        updateUI();
        if (!EnemyUI.gameObject.activeSelf)
            EnemyUI.gameObject.SetActive(true);

        if (HP <= 0)
        {
            StopAllCoroutines();
            state = AIStateId.Death;
            return;
        }
        else if (HP > 0) 
        state = AIStateId.ChasePlayer;


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

    private void DisableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }
        
    }

    private void EnableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }
    }
}

