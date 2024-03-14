using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] int maxSpawnGlobal;

    GameObject[] spawners;
    int currentSpawn;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            enemySpawner curSpawner = spawners[i].GetComponent<enemySpawner>();

            if (curSpawner.isPlayerinRange() && currentSpawn <= maxSpawnGlobal)
            {
                curSpawner.canSpawn = true;
            }
            else
            {
                curSpawner.canSpawn = false;
            }
        }
    }

    public void IncrementSpawnTotal()
    {
        currentSpawn++;
    }

    public void DecrementSpawnTotal(GameObject enemy)
    {
        enemySpawner parent = enemy.GetComponent<ZombieAI>().parentSpawner.GetComponent<enemySpawner>();
        for(int i = 0; i < parent.spawned.Length; i++)
        {
            if (enemy == parent.spawned[i])
            {
                parent.spawned[i] = null;
                parent.DecrementSpawnCount();
                parent.canSpawn = true;
                currentSpawn--;
                break;
            }
        }
    }
}
