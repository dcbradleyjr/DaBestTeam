using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] float fanSpeed;
    private void OnTriggerStay(Collider other)
    {
        IPushBack pushBack = other.GetComponent<IPushBack>();

        if (pushBack != null)
        {
            pushBack.pushBackDir(transform.up * fanSpeed * Time.deltaTime);
        }
    }
}
