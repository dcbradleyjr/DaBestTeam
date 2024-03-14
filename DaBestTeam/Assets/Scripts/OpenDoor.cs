using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour, IInteract
{
    [SerializeField] GameObject door;

    public string interactPrompt => ("Door " + cost);
    
    public int cost;
    public int currency;

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
        currency = gameManager.instance.GetCurrencyBalance();
    }

    public void interact()
    {
        if (currency >= cost)
        {
            gameManager.instance.SpendCurrency(cost);
            door.gameObject.SetActive(false);
        }
    }
}
