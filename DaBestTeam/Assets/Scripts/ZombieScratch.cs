using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScratch : MonoBehaviour
{
    public int damage { get;set; }
    [SerializeField] float timeToDestroy = 0.2f;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || other.CompareTag("Enemy"))
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damage);
            Destroy(gameObject);
        }
    }
}
