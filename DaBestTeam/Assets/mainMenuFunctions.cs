using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuFunctions : MonoBehaviour
{
    [Header("--Menu--")]
    [SerializeField] public GameObject loadingScreen;

    [Header("--Menu Buttons--")]
    public GameObject[] mainmenuButtons;
    public GameObject[] settingsMenuButtons;
    public GameObject creditScene;
    public GameObject backButton;
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject confirmText;

    [Header("--Text Credit Settings--")]
    [SerializeField] private TextMeshProUGUI itemInfoText;
    private int currentDisplayText = 0;
    [SerializeField][TextArea] private string[] itemInfo;
    [SerializeField] private float textSpeed = 0.01f;

    public bool inSettings;
    public bool inCredits;
    public Loader _loader;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    // Update is called once per frame
    void Update()
    {
        
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
            yield return new WaitForSecondsRealtime(textSpeed);
        }

    }

    public void mainMenuSettings()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        if (!inSettings)
        {
            ToggleButtons(mainmenuButtons);
            backButton.SetActive(true);
            ToggleButtons(settingsMenuButtons); 
            inSettings = true;
        }
    }

    public void Credits()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        if (!inCredits)
        {
            creditScene.SetActive(true);
            backButton.SetActive(true);
            ToggleButtons(mainmenuButtons);
            ActivateText();
            inCredits = true;
        }
    }

    public void MainMenuBack()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        
        creditScene.SetActive(false);
        backButton.SetActive(false);
        ToggleButtons(mainmenuButtons);

        if (inCredits)
        {
            creditScene.SetActive(false);
        }
        if (inSettings)
        {
            ToggleButtons(settingsMenuButtons);
        }        

        inSettings = false;
        inCredits = false;
    }
    public void StartNewGame()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        if (PlayerPrefs.HasKey("SavedSceneIndex") && PlayerPrefs.GetInt("SavedSceneIndex") > 1)
        {
            yesButton.SetActive(true);
            noButton.SetActive(true);
            confirmText.SetActive(true);
        }
        else
        {
            SaveManager.instance.ResetStats();
            _loader.LoadLevelButton();
        }
        
    }
    public void MainMenuNoButton()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        yesButton.SetActive(false);
        noButton.SetActive(false);
        confirmText.SetActive(false);
    }
    public void MainMenuYesButton()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        yesButton.SetActive(false);
        noButton.SetActive(false);
        confirmText.SetActive(false);
        SaveManager.instance.ResetStats();
        _loader.LoadLevelButton();
    }

    public void LoadGame()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
    }
    public void exit()// use for exit and quit
    {
        AudioManager.instance.PlaySFX("ButtonPress");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }
}
