using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
    public ThirdPersonController player;
    int HP;
    public int playerCurrency;
    public PlayerInput playerInput;
    public Animator playerAnim;
    public bool isPaused;

    public GameObject spawnPoint;

    public GameObject SaveStuff;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(gameObject);

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
            playerAnim = GameObject.FindWithTag("Player").GetComponent<Animator>();
            playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
            spawnPoint = GameObject.FindWithTag("SpawnPoint");
        }        
        
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

