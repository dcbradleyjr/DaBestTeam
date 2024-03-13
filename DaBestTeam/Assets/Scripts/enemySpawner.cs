using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] GameObject objToSpawn;
    [SerializeField] int maxSpawn;
    [SerializeField] float spawnDelay;
    [SerializeField] int spawnRadius;

    bool canSpawn = true;
    bool isSpawning;
    int spawnCount;
    
    void Start()
    {
        //updateGameGoal();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && !isSpawning && spawnCount < maxSpawn)
        {
            StartCoroutine(spawn());
        }
    }

    IEnumerator spawn()
    { 
        isSpawning = true;
        Vector3 randomPos = UnityEngine.Random.insideUnitSphere * spawnRadius;
        randomPos += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, spawnRadius, 1);
        Vector3 spawnPos = hit.position;
        GameObject spawned = Instantiate(objToSpawn, spawnPos, transform.rotation);
        float wep = UnityEngine.Random.Range(0f, 1f);
        if(wep >= 0.5f)
            spawned.GetComponent<ZombieAI>().canHoldWeapons = true;
        spawnCount++;
        yield return new WaitForSeconds(spawnDelay);
        isSpawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            canSpawn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canSpawn = false;
    }
}
