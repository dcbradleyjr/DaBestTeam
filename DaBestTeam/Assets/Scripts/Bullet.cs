using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("--Components--")]
    [SerializeField] Rigidbody rb;

    [Header("--Attributes--")]
    [SerializeField] int damageAmount;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] bool tracerBullet;
    [SerializeField] bool enemyBullet;
    [SerializeField] bool leadBullet;

    void Start()
    {
        if(enemyBullet & !leadBullet)
        rb.velocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed;

        else if(leadBullet && enemyBullet)
        {
            float distance = Vector3.Distance(gameManager.instance.player.transform.position, transform.position);
            float time = distance / speed;
            Vector3 predict = (gameManager.instance.player.transform.position + gameManager.instance.playerScript.GetVelocity() * time);
            rb.velocity = (predict - transform.position).normalized * speed;
        }
        else 
            //using transform right instead of forward because weapons face the X axis. 
        rb.velocity = transform.right * speed;

        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        if(tracerBullet && enemyBullet)
        rb.velocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {

        if ((enemyBullet && other.CompareTag("Enemy")) || (!enemyBullet && other.CompareTag("Player")) || other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
        }
        Destroy(gameObject);
    }
}
