using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] GameObject bloodSplat;
    [SerializeField] GameObject impact;
    [SerializeField] int damageAmount = 1;
    [SerializeField] float speed = 50f;
    [SerializeField] float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if(!hit && Vector3.Distance(transform.position, target) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;
        
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
            Instantiate(bloodSplat, transform.position, Quaternion.identity);
        }
        else
        {
          Instantiate(impact, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}