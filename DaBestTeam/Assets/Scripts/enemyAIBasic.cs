using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class enemyAIBasic: MonoBehaviour, IDamage
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
    }

    void Update()
    {
        if (playerInRange && canSeePlayer())
        {
            
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