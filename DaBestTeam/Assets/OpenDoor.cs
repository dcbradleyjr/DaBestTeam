using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour, IInteract
{
    [SerializeField] GameObject door;

    public void interact()
    {
        door.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        door = transform.parent.gameObject;

        // Enable the parent GameObject of this script
        door.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
