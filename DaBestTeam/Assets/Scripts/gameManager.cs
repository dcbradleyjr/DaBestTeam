using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    public Image playerHPBar;
    public GameObject FlashDMGPanel;
    public TMP_Text MagazineMax;
    public TMP_Text MagazineCount;
    public TMP_Text ReloadIndicator;
    public TMP_Text keyCountDisplay;
    public TMP_Text enemyCountDisplay;
    public TMP_Text keyObtained;


    public GameObject player;
    public Transform playerHead;
    public GameObject SpawnPoint;
    public playerController playerScript;

    public bool isPaused;
    int enemyCount;
    int keyCount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerHead = player.transform.Find("HeadPosition");
        SpawnPoint = GameObject.FindWithTag("SpawnPoint");
        playerScript = player.GetComponent<playerController>();
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

    public void updateEnemyCount(int amount)
    {
        enemyCount += amount;
        enemyCountDisplay.text = enemyCount.ToString();
        if (isObjectiveComplete())
        {
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
            youWin();
        }
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

    bool isObjectiveComplete()
    {
        if(enemyCount <= 0 && keyCount <= 0) return true;
        else return false;
    }

    IEnumerator pickupNotify(string myName)
    {
        keyObtained.text = myName + " Obtained";
        yield return new WaitForSeconds(1.3f);
        keyObtained.text = "";
    }
}

