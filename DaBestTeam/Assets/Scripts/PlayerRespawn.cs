using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public GameObject respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Respawning...");
            ThirdPersonController player = gameManager.instance.player;
            player.controller.enabled = false;
            player.transform.position = respawnPoint.transform.position;
            player.controller.enabled = true;
        }
    }
}
