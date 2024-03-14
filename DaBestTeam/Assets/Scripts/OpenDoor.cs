using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour, IInteract
{
    [SerializeField] GameObject door;

    public string interactPrompt => ("Door " + cost);
    
    public int cost;
    

    private gameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        door = transform.parent.gameObject;
        
        // Enable the parent GameObject of this script
        door.SetActive(true);

        gameManager = gameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void interact()
    {
        int playerCurrency = gameManager.GetCurrencyBalance();

        if (playerCurrency >= cost)
        {
            gameManager.instance.SpendCurrency(cost);
            door.gameObject.SetActive(false);
        }
    }
}
