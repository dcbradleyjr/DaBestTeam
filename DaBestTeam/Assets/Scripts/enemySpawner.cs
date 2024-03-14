using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] GameObject objToSpawn;
    [SerializeField] float spawnDelay;
    [SerializeField] int spawnRadius;
    [SerializeField] int playerRadius;

    public int maxSpawn;
    public bool canSpawn;
    public GameObject[] spawned;

    bool isSpawning;
    int spawnCount;
    
    void Start()
    {
        //updateGameGoal();
        Array.Resize(ref spawned, maxSpawn);
        //canSpawn = false;
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
        GameObject toSpawn = objToSpawn;
        float wep = UnityEngine.Random.Range(0f, 1f);
        if (wep >= 0.5f)
            toSpawn.GetComponent<ZombieAI>().canHoldWeapons = true;
        toSpawn.GetComponent<ZombieAI>().randomMesh = true;
        toSpawn.GetComponent<ZombieAI>().parentSpawner = gameObject;
        GameObject hasSpawned = Instantiate(toSpawn, spawnPos, transform.rotation);
        spawnCount++;
        SpawnManager.instance.IncrementSpawnTotal();
        for (int i = 0; i < maxSpawn; i++)
        {
            if (spawned[i] == null)
            {
                spawned[i] = hasSpawned;
                break;
            }
        }
        yield return new WaitForSeconds(spawnDelay);
        isSpawning = false;
    }

    public bool isPlayerInRange()
    {
        if ((gameManager.instance.player.transform.position - transform.position).magnitude <= playerRadius)
            return true;
        else
            return false;
    }

    public void DecrementSpawnCount()
    {
        spawnCount--;
    }
}
