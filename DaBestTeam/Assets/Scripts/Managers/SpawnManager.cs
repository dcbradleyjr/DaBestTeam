using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    int maxSpawnGlobal = 0;
    enemySpawner[] spawners;
    int currentSpawn;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if(instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        spawners = GameObject.FindObjectsOfType<enemySpawner>();
        for(int i = 0; i < spawners.Length; i++) 
        {
            maxSpawnGlobal += spawners[i].GetComponent<enemySpawner>().maxSpawn;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpawn <= maxSpawnGlobal)
        {
            for (int i = 0; i < spawners.Length; i++)
            {
                enemySpawner curSpawner = spawners[i];

                if (SceneManager.GetActiveScene().name != "MainMenu")
                {
                    if (!curSpawner.playerInRange && Vector3.Distance(curSpawner.transform.position, gameManager.instance.player.transform.position) <= 75)
                    {
                        curSpawner.canSpawn = true;
                    }
                    else
                    {
                        curSpawner.canSpawn = false;
                    } 
                }
                else
                {
                    curSpawner.canSpawn = true;
                }
            }
        }

    }

    public void IncrementSpawnTotal()
    {
        currentSpawn++;
    }

    public void DecrementSpawnTotal(GameObject enemy)
    {
        enemySpawner parent;
        if (enemy.TryGetComponent<ZombieAI>(out ZombieAI enemyAI))
        {
            parent = enemy.GetComponent<ZombieAI>().parentSpawner.GetComponent<enemySpawner>();
            for (int i = 0; i < parent.spawned.Length; i++)
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
        else
        {
            parent = enemy.GetComponent<ZombieBossAI>().parentSpawner.GetComponent<enemySpawner>();

            if (parent != null)
            {
                parent.spawned[0] = null;
                parent.DecrementSpawnCount();
                currentSpawn--;
            }
        }
    }

    public void ZombieAIReset()
    { 
        for(int i = 0; i < spawners.Length; i++) 
        {
            for (int j = 0; j < spawners[i].spawned.Length; j++)
            {
                if (spawners[i].spawned[j] != null)
                {
                    ZombieAI zombie = spawners[i].spawned[j].GetComponent<ZombieAI>();
                    zombie.state = ZombieAI.AIStateId.Roam;
                    zombie.agent.SetDestination(zombie.startingPosition);
                }
            }
        }
    }
}