using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startObjective : MonoBehaviour
{
    public GameObject keys;
    public GameObject enemies;
    public GameObject newSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            keys.SetActive(true);
            enemies.SetActive(true);
            gameManager.instance.canProgress = false;

            GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");
            spawnPoint.transform.position = newSpawnPoint.transform.position;
        }
    }
}
