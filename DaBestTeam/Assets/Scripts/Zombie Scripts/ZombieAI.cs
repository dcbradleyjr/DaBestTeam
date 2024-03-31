using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
    [Header("--Dropped Items--")]
    [SerializeField] GameObject[] pickupBox;

    [Header("--Components--")]
    [SerializeField] Transform headPosition;
    [SerializeField] GameObject[] meshMilitaryOptions;
    [SerializeField] GameObject[] meshZombie;
    [SerializeField] GameObject[] zombieWeapon;
    [SerializeField] GameObject floatingText;
    [SerializeField] Transform floatingSpawnPos;
    public bool canHoldWeapons;
    public bool randomMesh;
    [SerializeField] Transform HitPoint;
    [SerializeField] Transform HitBox;
    [SerializeField] float meleeRange;
    [SerializeField] GameObject zombieScratch;
    [SerializeField] Rigidbody[] _ragdollRigidbodies;
    [SerializeField]public  NavMeshAgent agent;
    [SerializeField] Animator anim;
    public AudioSource zombieSource;
    [SerializeField] Collider ZombieBase;

    [Header("--Stats--")]
    [Range(1, 50000)][SerializeField] int HP;
    [Range(1, 8)][SerializeField] int roamPauseTime;
    [Range(0, 45)][SerializeField] int roamDistance;
    [Range(30, 180)][SerializeField] int viewCone;
    [Range(1, 1000)][SerializeField] int targetFaceSpeed;
    [SerializeField] float attackCooldown;
    [SerializeField] float attackRange;
    [SerializeField] float dropRate;
    [SerializeField] int damageAmount;
    [SerializeField] int animSpeedTrans;
    [SerializeField] List<string> zombieHurtAudio;
    [SerializeField] List<string> zombieAttackAudio;
    [SerializeField] List<string> zombieChaseAudio;
    [SerializeField] List<string> zombieRoamAudio;

    [Header("--UI--")]
    public UnityEngine.UI.Image HealthBar;
    public GameObject EnemyUI;


    [Header("--Animations--")]
    public List<string> attackList = new List<string>();
    public GameObject parentSpawner; //needed for SpawnManager

    [SerializeField] private AIStateId _state = AIStateId.Roam;

    int HPOriginal;
    public Vector3 startingPosition;
    Vector3 playerDir;
    public float stoppingDistanceOrig;

    float pathLockedTimer;

    public enum AIStateId { Roam = 1, ChasePlayer = 2, Attack = 3, Death = 4 };

    public void Awake()
    {
        state = AIStateId.Roam;
        HPOriginal = HP;
        startingPosition = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;

        updateUI();
        chooseRandomMesh();
        chooseWeapon();
        DisableRagdoll();
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        /*AudioManager.instance.ZombieSpawnAudio(this);*/

        zombieSource = GetComponent<AudioSource>();
        if (zombieSource == null)
        {
            // If AudioSource component is not found, add it
            zombieSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Update()
    {

        if (playerInRange)
        {
            if (gameManager.instance != null && gameManager.instance.player != null)
            {

                playerDir = gameManager.instance.player.transform.position - HitPoint.position;
            }
            else
            {
                playerDir = Vector3.zero;
            } 
        }

        if (AIStateId.Roam == state)
        {
            pathLockedTimer -= Time.deltaTime;
        }


        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));
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

            }

        }
    }

    private bool isDead;
    private bool isAttacking;
    private bool playerInRange;
    private bool isRoaming;
    private bool isChasing;

    public IEnumerator roamState()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(0.5f);
            //AudioManager.instance.PlayZombieSFX("ZombieRoam");
            if (!agent.pathPending && (agent.remainingDistance < 0.1f || pathLockedTimer <= 0f))
            {
                isChasing = false;
                isRoaming = false;
                pathLockedTimer = 10f;
                Vector3 randomPos = Random.insideUnitSphere * roamDistance;
                //Debug.Log("randomPos :" + randomPos);
                randomPos += startingPosition;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomPos, out hit, roamDistance, 1);
                //Debug.Log("hit positon: " + hit.position);
                agent.SetDestination(hit.position);

                this.PlayRandomRoamSound();      
                
            }
            yield return new WaitForSeconds(roamPauseTime);
        }

    }
    public IEnumerator chaseState()
    {
        while (true)
        {
            
            faceTarget();
            //AudioManager.instance.PlayZombieSFX("ZombieChase");
            float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);
            if (distanceToPlayer < attackRange)
            {
                // Transition to attack state
                StartCoroutine(attackState());
                // Exit chase state
                yield break;
            }

            // Chase the player
            agent.SetDestination(gameManager.instance.player.transform.position);
            if (!isChasing)
            {
                this.PlayRandomChaseSound();
                isChasing = true;
            }
            yield return null;
        }
    }
    public IEnumerator attackState()
    {
        while (true)
        {
            
            isChasing = false;
            if (!isAttacking)
            {
                isAttacking = true;
                PlayRandomAttackSound();
                PlayRandomAttackAnim();
                Debug.Log("Attacked");
                yield return new WaitForSeconds(attackCooldown);
                isAttacking = false;
                state = AIStateId.ChasePlayer;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void SpawnScratch()
    {
        GameObject scratch = Instantiate(zombieScratch, HitBox.position, HitBox.rotation);
        scratch.GetComponent<ZombieScratch>().damage = damageAmount;
    }
    public IEnumerator deathState()
    {
        if (!isDead)
        {
            isDead = true;
            PlayRandomHurtSound();

            gameManager.instance.EarnCurrency(50,"Zombie Kill", Color.white);

            this.EnemyUI.SetActive(false);

            this.GetComponent<Animator>().enabled = false;

            this.GetComponent<NavMeshAgent>().enabled = false;

            this.ZombieBase.enabled = false;
            

            EnableRagdoll();

            bool dropItem = Random.value * 100f <= dropRate;// Drop rate set on SerializedField .01 = 1%
            Debug.Log(dropItem);
            if (dropItem)
            {
                int i = Random.Range(0, pickupBox.Length);
                Instantiate(pickupBox[i], transform.position + new Vector3(0,1,0), Quaternion.identity);
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
            faceTarget();
            if (state != AIStateId.Death)
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
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }
    public void takeDamage(int amount, bool headshot)
    {
        anim.SetTrigger("Hurt");
        PlayRandomHurtSound();
        HP -= amount;
        Debug.Log("Damage");
        updateUI();

        if (!EnemyUI.gameObject.activeSelf)
            EnemyUI.gameObject.SetActive(true);

        if (floatingText)
        {
            if (headshot)
                ShowFloatingText(amount, true);
            else
                ShowFloatingText(amount, false);
        }

        if (HP <= 0)
        {
            StopAllCoroutines();
            state = AIStateId.Death;
            return;
        }
        else if (HP > 0 && !isAttacking)
        {
            if (headshot)
            gameManager.instance.EarnCurrency(2, "Critical Shot", Color.yellow);

            state = AIStateId.ChasePlayer;
        }
    }
    public void pushBackDir(Vector3 dir)
    {
        agent.enabled = false;
        agent.velocity += (dir / 2);
        agent.enabled = true;
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
    public void PlayRandomHurtSound()
    {
        string randomClipName = zombieHurtAudio[Random.Range(0, zombieHurtAudio.Count)];
        AudioManager.instance.PlayZombieSFX(randomClipName, zombieSource);
    }
    private void PlayRandomAttackSound()
    {
        string randomClipName = zombieAttackAudio[Random.Range(0, zombieAttackAudio.Count)];
        AudioManager.instance.PlayZombieSFX(randomClipName, zombieSource);
    }
    private void PlayRandomChaseSound()
    {
        string randomClipName = zombieChaseAudio[Random.Range(0, zombieChaseAudio.Count)];
        AudioManager.instance.PlayZombieSFX(randomClipName, zombieSource);
        Debug.Log("ChaseSound");
    }
    public void PlayRandomRoamSound()
    {
        string randomClipName = zombieRoamAudio[Random.Range(0, zombieRoamAudio.Count)];
        AudioManager.instance.PlayZombieSFX(randomClipName, zombieSource);
        Debug.Log("RoamSound");
    }
    public void PlayRandomAttackAnim()
    {
        int randomIndex = UnityEngine.Random.Range(0, attackList.Count);
        string randomString = attackList[randomIndex];
        Debug.Log("AttackSound");
        anim.SetTrigger(randomString);
    }
    void ShowFloatingText(int value, bool headshot)
    {
        GameObject text = GameObject.Instantiate(floatingText, floatingSpawnPos.position, Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = value.ToString();
        if (headshot)
            text.GetComponent<TextMeshPro>().color = Color.yellow;
    }

}

