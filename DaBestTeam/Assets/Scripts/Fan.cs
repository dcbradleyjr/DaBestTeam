using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{


    Animator anim;

   public void fanAnimation()
    {
        Debug.Log("I am trying to spin");
        anim.SetTrigger("FanSpin");
        
    }
    
}
