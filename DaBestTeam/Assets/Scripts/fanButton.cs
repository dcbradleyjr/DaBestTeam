using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanButton : MonoBehaviour, IInteract
{

    [SerializeField] Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void interact() 
    {
        Debug.Log("I am interacting");
        anim.SetTrigger("FanSpin");
    }
}
