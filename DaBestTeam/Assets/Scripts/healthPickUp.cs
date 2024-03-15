using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickUp : MonoBehaviour
{
    public ThirdPersonController thirdPersonController;

    [SerializeField] int healAmount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             IHealth health = other.GetComponent<IHealth>();

            if (health != null)
            {
                thirdPersonController.HealPlayer(healAmount);
            }
            Destroy(gameObject);
        }
            
    }
}
