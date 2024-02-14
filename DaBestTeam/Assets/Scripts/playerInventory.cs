using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class playerInventory : MonoBehaviour
{
    [SerializeField] GameObject[] inventoryItems;


    /*
    bool[] inventory;

    // Start is called before the first frame update
    void Start()
    {

        inventory = new bool[inventoryItems.Length];
        for(int i = 0; i < inventory.Length; i++)
            inventory[i] = false;

    }

    //Add an item to the inventory by checking the object name
    public void AddToInventory(string objectName)
    { 
        for( int i = 0; i < inventory.Length; i++) 
        {
            if (inventoryItems[i].name == objectName)
            {
                inventory[i] = true;
            }
        }
    }

    //Remove objects from the inventory by checking the object name
    public void RemoveFromInventory(string objectName)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventoryItems[i].name == objectName)
            {
                inventory[i] = false;
            }
        }
    }
    */

}
