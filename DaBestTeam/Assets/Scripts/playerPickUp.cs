using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPickUp : MonoBehaviour
{
    [SerializeField] int healAmount;
    [SerializeField] int MagazineMultiplier;
    [SerializeField] float lifeSpan = 12f;
    [SerializeField] GameObject effect;

    public bool Health;
    public bool GunAmmo;

    private void Start()
    {
        Destroy(gameObject, lifeSpan);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             IHealth health = other.GetComponent<IHealth>();

            if (other.CompareTag("Player")&&Health)
                gameManager.instance.player.HealPlayer(healAmount);
            if (other.CompareTag("Player") && GunAmmo)
            {
                int value = MagazineMultiplier * WeaponSlotManager.instance.Gun.GetClipSize();
                WeaponSlotManager.instance.Gun.AddAmmo(value);
            }
            GameObject spawnEffect = GameObject.Instantiate(effect, transform.position - new Vector3(0,1f,0),transform.rotation);
            Destroy(spawnEffect,0.5f);
            Destroy(gameObject);
        }    
    }
}
