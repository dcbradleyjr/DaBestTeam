using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    [Header("--Components--")]
    public static gameManager instance;
    [SerializeField] Camera mainCamera;

    [Header("--HUD--")]
    public Image playerHPBar;
    public Image playerStaminaBar;

    /*    public GameObject FlashDMGPanel;
        public GameObject FlashHealPanel;*/

    [Header("--Player Info--")]
    public GameObject player;
    int HP;
    public int playerCurrency;

    public bool isPaused;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        if(instance != this)
            Destroy(gameObject);

        player = GameObject.FindWithTag("Player");
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
    }
   
    public void statePaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

    }

    public void stateUnpaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
        UIManager.instance.menuActive.SetActive(false);
        UIManager.instance.menuActive = null;

    }

    public void youWin()
    {
        // you win!!
        statePaused();
        UIManager.instance.menuActive = UIManager.instance.menuWin;
        UIManager.instance.menuActive.SetActive(true);
    }

    public void youLose()
    {
        //you lose!!
        statePaused();
        UIManager.instance.menuActive = UIManager.instance.menuLose;
        UIManager.instance.menuActive.SetActive(true);
    }
    
   
    // Method to earn currency
    public void EarnCurrency(int amount)
    {
        playerCurrency += amount;
    }

    // Method to spend currency
    public bool SpendCurrency(int amount)
    {
        if (playerCurrency >= amount)
        {
            Debug.LogError("Thanks");
            playerCurrency -= amount;
            return true; // Transaction successful
        }
        else
        {
            Debug.Log("You broke");
            return false; // Insufficient funds
        }
    }

    // Method to check current currency balance
    public int GetCurrencyBalance()
    {
        return playerCurrency;
    }

    

}

