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

        enemySpawner parent = enemy.GetComponent<ZombieAI>().parentSpawner.GetComponent<enemySpawner>();
        if (parent == null)
        {
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
    }
}
