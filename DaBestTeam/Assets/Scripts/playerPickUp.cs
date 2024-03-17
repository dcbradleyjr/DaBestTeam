using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPickUp : MonoBehaviour
{
    [SerializeField] int healAmount;
    [SerializeField] int PistolAmmoAmount;
    [SerializeField] int GunAmmoAmount;

    public bool Health;
    public bool GunAmmo;
    public bool PistolAmmo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             IHealth health = other.GetComponent<IHealth>();

            if (other.CompareTag("Player")&&Health)
                gameManager.instance.player.HealPlayer(healAmount);
            if (other.CompareTag("Player")&& GunAmmo)
                WeaponSlotManager.instance.Gun.AddAmmo(GunAmmoAmount);

            Destroy(gameObject);
        }
            
    }
}
