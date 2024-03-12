using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;

public class gameManager : MonoBehaviour
{
    [Header("--Components--")]
    public static gameManager instance;

    [Header("--Menu--")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    [Header("--HUD--")]
   /* public Image playerHPBar;
    public Image playerStaminaBar;
    public GameObject FlashDMGPanel;
    public GameObject FlashHealPanel;
    public TMP_Text MagazineMax;
    public TMP_Text MagazineCount;
    public TMP_Text weaponName;
    public TMP_Text ReloadIndicator;
    public TMP_Text AmmoDivider;*/

    

    [Header("--Player Info--")]
    public GameObject player;
    public GameObject SpawnPoint;


    public bool isPaused;

    int enemyCount;
    int keyCount;
    int levelCount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        SpawnPoint = GameObject.FindWithTag("SpawnPoint");

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

   /* public void updateEnemyCount(int amount)
    {
        enemyCount += amount;
        enemyCountDisplay.text = enemyCount.ToString();
        if (isObjectiveComplete())
        {
            canProgress = true;
            if (currentLevel == levelCount)
                youWin();
        }
    }
    public void updateKeyCount(int amount,string myName)
    {
        keyCount += amount;
        keyCountDisplay.text = keyCount.ToString();
        if(amount == -1) 
        StartCoroutine(pickupNotify(myName));


        if(isObjectiveComplete())
        {
            canProgress = true;
            if (currentLevel == levelCount)
                youWin();
        }
    }*/

   
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

  
}

