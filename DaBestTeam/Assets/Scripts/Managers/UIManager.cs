using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("--Menu Buttons--")]
    public GameObject[] PausemenuButtons;
    public GameObject[] PausesettingsMenuButtons;
    public GameObject pausebackButton;

    [Header("--Menu--")]
    [SerializeField] public GameObject menuActive;
    [SerializeField] public GameObject menuPause;
    [SerializeField] public GameObject menuWin;
    [SerializeField] public GameObject menuLose;
    [SerializeField] public GameObject loadingScreen;    

    [Header("--HUD--")]
    [SerializeField] public GameObject StaminaDisplay;
    [SerializeField] public GameObject HPDisplay;
    [SerializeField] public GameObject AmmoDisplay;
    [SerializeField] public GameObject CurrencyDisplay;
    [SerializeField] public TextMeshProUGUI currency;
    [SerializeField] public TextMeshProUGUI ammoCurrentDisplay;
    [SerializeField] public TextMeshProUGUI weaponNameDisplay;
    [SerializeField] public TextMeshProUGUI ammoReloadDisplay;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && menuActive == null) //ESC button
        {
            gameManager.instance.statePaused(); 
            menuActive = menuPause;
            menuActive.SetActive(gameManager.instance.isPaused);
        }

        currency.text = gameManager.instance.playerCurrency.ToString();
    }
    public void ToggleButtons(GameObject[] buttons)
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(!button.activeSelf);
        }
    }
   
}
