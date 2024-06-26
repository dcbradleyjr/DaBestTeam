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


public class ZombieBossAI : MonoBehaviour, IDamage
{
    [Header("--Dropped Items--")]
    [SerializeField] GameObject[] pickupBox;

    [Header("--Components--")]
    [SerializeField] Transform headPosition;
    [SerializeField] GameObject[] meshZombie;
    [SerializeField] GameObject[] zombieWeapon;
    public bool canHoldWeapons;
    public bool randomMesh;
    [SerializeField] Transform HitPoint;
    [SerializeField] Transform HitBox;
    [SerializeField] float meleeRange;
    [SerializeField] GameObject zombieScratch;
    [SerializeField] GameObject shockwaveDetonate;
    [SerializeField] Rigidbody[] _ragdollRigidbodies;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;

    [Header("--Stats--")]
    [Range(1, 50000)][SerializeField] int HP;
    [Range(1, 8)][SerializeField] int roamPauseTime;
    [Range(0, 45)][SerializeField] int roamDistance;
    [Range(30, 180)][SerializeField] int viewCone;
    [Range(1, 1000)][SerializeField] int targetFaceSpeed;
    [SerializeField] float attackCooldown;
    [SerializeField] float attackRange;
    [SerializeField] float attackAnimSpeed;
    [SerializeField] float dropRate;
    [SerializeField] int damageAmount;


    [Header("--UI--")]
    public UnityEngine.UI.Image HealthBar;
    public GameObject EnemyUI;

    public GameObject parentSpawner; //needed for SpawnManager

    [SerializeField] private AIStateId _state = AIStateId.Roam;

    int HPOriginal;
    float angleToPlayer;

    public Vector3 startingPosition;
    Vector3 playerDir;
    float stoppingDistanceOrig;
    bool destChosen;
    private bool playerInRange;

    public int shockwaveAnimationTimeOffset;
    public int stompDist;
    public float countdownTimer;
    public bool isBigBoss;

    int hurtAnim;
    int attackAnim;
    int walkAnim;
    bool animOrignal;

    [Header("--Attack Info--")]
    ParticleSystem shockwavePS;
    float distanceToPlayer;
    bool isStomping;
    float timeBetweenAttacks;

    public enum AIStateId { Roam = 1, ChasePlayer = 2, Attack = 3, Death = 4, Stomp = 5 };

    public void Awake()
    {
        state = AIStateId.Roam;
        HPOriginal = HP;
        //startingPosition = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;

        updateUI();
        //chooseRandomMesh();
        chooseWeapon();
        DisableRagdoll();

        animOrignal = anim;

        attackAnim = Animator.StringToHash("Zombie@Attack01");
        hurtAnim = Animator.StringToHash("Zombie@Damage01");
        walkAnim = Animator.StringToHash("Zombie@Walk01");

        shockwavePS = transform.Find("Shockwave").GetComponent<ParticleSystem>();
        timeBetweenAttacks = countdownTimer;

        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    public void Update()
    {
        /*if(state != AIStateId.Death)
        takeDamage(1);*/
        playerDir = gameManager.instance.player.transform.position - HitPoint.position;
    }
    public AIStateId state
    {
        get { return _state; }

        set
        {

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
                    StartCoroutine(deathState());
                    break;
                case AIStateId.Stomp:
                    UpdateAttackState();
                    break;
            }
        }
    }

    private bool isDead;
    private bool isAttacking;
    private bool isDamage;

    public IEnumerator roamState()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            //AudioManager.instance.PlaySFX("ZombieRoam");
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                Vector3 randomPos = Random.insideUnitSphere * roamDistance;
                //Debug.Log("randomPos :" + randomPos);
                randomPos += startingPosition;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomPos, out hit, roamDistance, 1);
                //Debug.Log("hit positon: " + hit.position);
                agent.SetDestination(hit.position);

            }

            yield return new WaitForSeconds(roamPauseTime);
        }

    }
    public IEnumerator chaseState()
    {
        while (true)
        {
            faceTarget();
            //AudioManager.instance.PlaySFX("ZombieChase");
            float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);
            if (distanceToPlayer < attackRange)
            {
                Debug.Log(distanceToPlayer + "Player Distance");
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
            faceTarget();

            //angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

            agent.SetDestination(gameManager.instance.player.transform.position);


            if (!isAttacking)
            {
                isAttacking = true;
                float shockwaveAttackChance = Random.Range(0f, 1f);
                if (shockwaveAttackChance < 0.5f)
                {
                    state = AIStateId.Stomp;
                    break;
                }
                
                else
                {
                    anim.SetTrigger("Attack");
                    yield return new WaitForSeconds(attackAnimSpeed/3);

                    //AudioManager.instance.PlaySFX("ZombieAttack");

                    GameObject scratch = Instantiate(zombieScratch, HitBox.position, HitBox.rotation);
                    scratch.GetComponent<ZombieScratch>().damage = damageAmount;

                    yield return new WaitForSeconds(attackAnimSpeed / 5);

                    state = AIStateId.ChasePlayer;

                    yield return null;
                }

                isAttacking = false;
            }

            yield return null;
        }
    }
    public IEnumerator deathState()
    {
        if (!isDead)
        {
            isDead = true;

            //AudioManager.instance.PlaySFX("ZombieDeath");

            gameManager.instance.EarnCurrency(50, "Zombie Boss Kill", Color.red);

            this.EnemyUI.SetActive(false);

            this.GetComponent<Animator>().enabled = false;

            this.GetComponent<NavMeshAgent>().enabled = false;

            this.GetComponent<Collider>().enabled = false;

            EnableRagdoll();

            bool dropItem = Random.value <= dropRate;// Drop rate set on SerializedField .01 = 1%

            if (dropItem)
            {
                int i = Random.Range(0, pickupBox.Length);
                Instantiate(pickupBox[i], HitPoint.position, Quaternion.identity);
            }

            if (SpawnManager.instance != null)
            {
                SpawnManager.instance.DecrementSpawnTotal(this.gameObject); //spawnManager handles things while this dies
            }

            Destroy(gameObject, 2f);
            yield return null;
        }
    }

    IEnumerator stompShockwave()
    {
        isAttacking = true;
        anim.SetTrigger("Stomp");
        Debug.Log("stomp");
        yield return new WaitForSeconds(3f);
        state = AIStateId.ChasePlayer;
        isAttacking = false;
        yield return null;
    }

    void shockwaveAnimation()
    {
        Debug.Log("anim play");
        shockwavePS.Play();
        GameObject detonate = Instantiate(shockwaveDetonate, transform.position, transform.rotation);
        detonate.GetComponent<ZombieScratch>().damage = damageAmount;
    }

    void UpdateAttackState()
    {
        distanceToPlayer = Vector3.Distance(gameManager.instance.player.transform.position, transform.position);
        faceTarget();

            switch (state)
            {
                case AIStateId.ChasePlayer:
                {
                    if (distanceToPlayer < stompDist)
                    {
                        state = AIStateId.Stomp;
                    }
                }   break;
                case AIStateId.Stomp:
                {
                    countdownTimer = timeBetweenAttacks;
                    StartCoroutine(stompShockwave());
                }   break;
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
            faceTarget();
            if (state != AIStateId.Death)
                state = AIStateId.ChasePlayer;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
        agent.stoppingDistance = 0;
    }
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }
    public void takeDamage(int amount, bool headshot)
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

        if (HP > 0 && !isAttacking)
        {
            state = AIStateId.ChasePlayer;
        }
    }

    public void chooseRandomMesh()
    {
        if (randomMesh && meshZombie.Length > 0)
        {
            int randomIndex = Random.Range(0, meshZombie.Length);
            meshZombie[randomIndex].gameObject.SetActive(true);
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

