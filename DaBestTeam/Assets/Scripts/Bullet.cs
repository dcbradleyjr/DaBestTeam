using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] bool tracerBullet;
    [SerializeField] bool enemyBullet;



    void Start()
    {
        if(enemyBullet)
        rb.velocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed;
        else 
        rb.velocity = transform.forward * speed;

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
