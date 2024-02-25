using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    Animator LinkedAnimator;
    
    [SerializeField] float fanSpeed;
    private void OnTriggerEnter(Collider other)
    {

        LinkedAnimator.SetTrigger("FanSpin");

        IPushBack pushBack = other.GetComponent<IPushBack>();

        if (pushBack != null)
        {
            
            pushBack.pushBackDir(transform.up * fanSpeed * Time.deltaTime);
        }
    }
}
