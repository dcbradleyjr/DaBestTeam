/*using System.Collections;
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
        if (enemyBullet)
        { 
            if(leadBullet)
                rb.velocity = CalculateShotLead();
            else
                rb.velocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed;
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

    private Vector3 CalculateShotLead()
    {
        float timeToPlayer = (gameManager.instance.player.transform.position - transform.position).magnitude / speed; //calculate how long the bullet will take to reach the player
        Vector3 futurePos = gameManager.instance.player.transform.position + (gameManager.instance.playerScript.GetVelocity() * timeToPlayer); //calculate the player's future position
        Vector3 aimLoc = (futurePos - transform.position).normalized; //calculate where to aim at
        return aimLoc.normalized * speed;
    }
}
*/