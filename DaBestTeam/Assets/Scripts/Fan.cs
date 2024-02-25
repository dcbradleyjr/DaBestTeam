using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{


    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }


    public void fanAnimation()
    {
        Debug.Log("I am trying to spin");
        anim.SetTrigger("FanSpin");
    }
    
}
