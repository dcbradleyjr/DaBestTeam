using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class enemyAIBoss : MonoBehaviour, IDamage
{
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPosition;
    [SerializeField] Transform headPosition;
    [Range(1, 10)][SerializeField] int animSpeedTrans;

    [SerializeField] int HP;
    [SerializeField] int viewCone;
    [SerializeField] int targetFaceSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject leadBullet;
    [SerializeField] GameObject bossBullet;
    [Range(0f, 1f)][SerializeField] float leadchance;
    [SerializeField] float shootRate;
    [SerializeField] int countToTracer;
    [SerializeField] int tracerShotsToFire;

    public UnityEngine.UI.Image HealthBar;
    public GameObject EnemyUI;

    bool isShooting;
    float angleToPlayer;
    Vector3 playerDirection;
    int HPOriginal;
    int shotsFired;
    int tracerFired;

    void Start()
    {
        gameManager.instance.updateEnemyCount(1);
        HPOriginal = HP;
        updateUI();
        shotsFired = 0;
    }

    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));
        chasePlayer();
    }

    void chasePlayer()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);

        playerDirection = gameManager.instance.playerHead.position - headPosition.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        Debug.DrawRay(headPosition.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(headPosition.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {

                if (!isShooting)
                {
                    if (shotsFired < countToTracer)
                    {
                        float doesLead = Random.Range(0f, 1.0f);
                        if (doesLead < leadchance)
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
                        if (tracerFired < tracerShotsToFire)
                            StartCoroutine(tracerShoot());
                        else
                        {
                            shotsFired = 0;
                            tracerFired = 0;
                        }
                    }
                }
                if (agent.remainingDistance < agent.stoppingDistance)
                    faceTarget();
            }
        }
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDirection.x, transform.position.y, playerDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }

    public void takeDamage(int amount)
    {
        AudioManager.instance.enemyHurtSound();
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
        AudioManager.instance.enemyShootSound();
        isShooting = true;
        Instantiate(bullet, shootPosition.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        shotsFired++;
        isShooting = false;
    }

    IEnumerator leadShoot()
    {
        AudioManager.instance.enemyShootSound();
        isShooting = true;
        Instantiate(leadBullet, shootPosition.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        shotsFired++;
        isShooting = false;
    }

    IEnumerator tracerShoot()
    {
        AudioManager.instance.enemyShootSound();
        isShooting = true;
        Instantiate(bossBullet, shootPosition.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        tracerFired++;
        isShooting = false;
    }

    void updateUI()
    {
        HealthBar.fillAmount = (float)HP / HPOriginal;
    }
}
