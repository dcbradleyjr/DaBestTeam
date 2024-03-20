using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScratch : MonoBehaviour
{
    public int damage { get;set; }
    [SerializeField] float timeToDestroy = 0.2f;

    void Awake()
    {
        Debug.Log("SpawnScratch");
        Destroy(gameObject, timeToDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || !other.CompareTag("Player"))
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            Debug.Log(other.name);
            dmg.takeDamage(damage);
            Destroy(gameObject);
        }
    }
}
