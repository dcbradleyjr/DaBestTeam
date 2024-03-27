using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRoamReset : MonoBehaviour
{
    public bool hasTriggered = false;

    void Start()
    {
        Debug.Log("Can Reset");
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            SpawnManager.instance.ZombieAIReset();
        hasTriggered = true;
    }
}
