using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] GameObject bloodSplat;
    [SerializeField] GameObject impact;
    [SerializeField] float speed = 50f;
    private float _timeToDestroy;

    public float timeToDestroy 
    {
        get { return _timeToDestroy; } 
        set {  _timeToDestroy = value; Destroy(gameObject, timeToDestroy); }
    }
    public int damageAmount { get; set; }
    public Vector3 target { get; set; }
    public bool hit { get; set; }

    public bool piercingShot { get; set; }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (!hit && Vector3.Distance(transform.position, target) < 0.1f)
        {
            //if (!piercingShot)
            //    Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || other.CompareTag("Player"))
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
            GameObject blood = Instantiate(bloodSplat, transform.position, Quaternion.identity);
            Destroy(blood, 0.5f);
            WeaponSlotManager.instance.StartHitmarker();
        }
        else
        {
            GameObject spark = Instantiate(impact, transform.position, Quaternion.identity);
            Destroy(spark, 0.5f);
        }

        //if(!piercingShot)
        //Destroy(gameObject);
    }
}
