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
    public GameObject[] mainmenuButtons;
    public GameObject[] PausemenuButtons;
    public GameObject[] settingsMenuButtons;
    public GameObject[] PausesettingsMenuButtons;
    public GameObject creditScene;
    public GameObject backButton;
    public GameObject pausebackButton;

    [Header("--Menu--")]
    [SerializeField] public GameObject menuActive;
    [SerializeField] public GameObject menuPause;
    [SerializeField] public GameObject menuWin;
    [SerializeField] public GameObject menuLose;
    [SerializeField] public GameObject MainMenu;
    [SerializeField] public GameObject SkillsMenu;
    [SerializeField] public GameObject loadingScreen;

    [Header("--Text Credit Settings--")]
    [SerializeField] private TextMeshProUGUI itemInfoText;
    private int currentDisplayText = 0;
    [SerializeField][TextArea] private string[] itemInfo;
    [SerializeField] private float textSpeed = 0.01f;

    [Header("--HUD--")]
    [SerializeField] public GameObject StaminaDisplay;
    [SerializeField] public GameObject HPDisplay;
    [SerializeField] public GameObject AmmoDisplay;
    [SerializeField] public GameObject CurrencyDisplay;
    [SerializeField] public TextMeshProUGUI currency;
    [SerializeField] public TextMeshProUGUI ammoCurrentDisplay;
    [SerializeField] public TextMeshProUGUI weaponNameDisplay;
    [SerializeField] public TextMeshProUGUI ammoReloadDisplay;

    [SerializeField] public GameObject DamageScreenOne;
    [SerializeField] public GameObject DamageScreenTwo;
    [SerializeField] public GameObject DamageScreenThree;


    public Loader _loader;
    public bool inSettings;
    public bool inCredits;
    public UpgradeSkills upgradeSkills;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(gameObject);

        _loader = GameObject.FindWithTag("Loader").GetComponent<Loader>();
        _loader.isLoading = false;

        upgradeSkills = FindAnyObjectByType<UpgradeSkills>();
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

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            MainMenu.gameObject.SetActive(false);
            if (!gameManager.instance.isPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                CurrencyDisplay.gameObject.SetActive(true);
                StaminaDisplay.gameObject.SetActive(true);
                AmmoDisplay.gameObject.SetActive(true);
                HPDisplay.gameObject.SetActive(true);
            }
            else
            {
                CurrencyDisplay.gameObject.SetActive(false);
                StaminaDisplay.gameObject.SetActive(false);
                AmmoDisplay.gameObject.SetActive(false);
                HPDisplay.gameObject.SetActive(false);
            }
        }
        else
        {
            menuActive = MainMenu;
            if (!gameManager.instance.isPaused && !_loader.isLoading)
                gameManager.instance.statePaused();
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
    public void ActivateText()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        for (int i = 0; i < itemInfo[currentDisplayText].Length + 1; i++)
        {
            itemInfoText.text = itemInfo[currentDisplayText].Substring(0, i);
            yield return new WaitForSeconds(textSpeed);
        }

    }
}
