using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUp : MonoBehaviour, IInteract
{
    // Start is called before the first frame update

    [SerializeField] string myName;
    void Start()
    {
       gameManager.instance.updateKeyCount(1, myName);

    }


    public void interact()
    {
        gameManager.instance.updateKeyCount(-1, myName);
        Destroy(gameObject);
    }
}
