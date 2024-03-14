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

    [Header("--Menu--")]
    [SerializeField] GameObject menuActive;
    [SerializeField] public GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] public GameObject MainMenu;
    [SerializeField] public GameObject SettingsMenu;

    [Header("--HUD--")]
    public Image playerHPBar;
    public Image playerStaminaBar;
/*    public GameObject FlashDMGPanel;
    public GameObject FlashHealPanel;*/

    [Header("--Player Info--")]
    public GameObject player;
    int HP;
    int playerCurrency;

    
    public bool isPaused;

    int enemyCount;
    int keyCount;
    int levelCount;

    private int playerPocket;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && menuActive == null) //ESC button
        {
            statePaused();
            menuActive = menuPause;
            menuActive.SetActive(isPaused);
        }
        
        if (SceneManager.GetActiveScene().name != "MainMenu" && !isPaused)
        {
            MainMenu.gameObject.SetActive(false);
        }
        
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
        Cursor.visible = false; ;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;


    }



    public void youWin()
    {
        // you win!!
        menuActive = menuWin;
        menuActive.SetActive(true);
        statePaused();
    }

    public void youLose()
    {
        //you lose!!
        statePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
    
    public void takeDamage(int damageAmount)
    {
        HP -= damageAmount;
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
            playerCurrency -= amount;
            return true; // Transaction successful
        }
        else
        {
            return false; // Insufficient funds
        }
    }

    // Method to check current currency balance
    public int GetCurrencyBalance()
    {
        return playerCurrency;
    }

    

}

