using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    
    Loader loader;
    public void resume()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        gameManager.instance.stateUnpaused();
        
    }

    public void restart()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpaused();
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

    public void respawn()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        gameManager.instance.stateUnpaused();
    }

    public void StartGame()
    {
        AudioManager.instance.PlaySFX("ButtonPress");


        AudioManager.instance.SetSFXVolume();
        AudioManager.instance.SetMusicVolume();
    }

    public void Settings()
    {
        
        AudioManager.instance.PlaySFX("ButtonPress");
        if (!UIManager.instance.inSettings)
        {
            
            UIManager.instance.inSettings = true;
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                UIManager.instance.ToggleButtons(UIManager.instance.mainmenuButtons);
                UIManager.instance.backButton.SetActive(true);
                UIManager.instance.ToggleButtons(UIManager.instance.settingsMenuButtons);
            }
            if(SceneManager.GetActiveScene().name != "MainMenu")            
            {
                UIManager.instance.ToggleButtons(UIManager.instance.PausemenuButtons);
                UIManager.instance.pausebackButton.SetActive(true);
                UIManager.instance.ToggleButtons(UIManager.instance.PausesettingsMenuButtons);
            }
            Debug.Log("Here");
            
        }
        
       

    }


    public void MainMenuBack()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            UIManager.instance.MainMenu.gameObject.SetActive(true);
            UIManager.instance.creditScene.SetActive(false);
            UIManager.instance.backButton.SetActive(false);
            UIManager.instance.ToggleButtons(UIManager.instance.mainmenuButtons);
            UIManager.instance.ToggleButtons(UIManager.instance.settingsMenuButtons);
            
        }
        if (UIManager.instance.inSettings && SceneManager.GetActiveScene().name != "MainMenu")
        {
            UIManager.instance.ToggleButtons(UIManager.instance.PausemenuButtons);
            UIManager.instance.pausebackButton.SetActive(false);
            UIManager.instance.ToggleButtons(UIManager.instance.PausesettingsMenuButtons);
        }
        if (!UIManager.instance.inSettings)
        {
            UIManager.instance.ToggleButtons(UIManager.instance.settingsMenuButtons); 
        }
        UIManager.instance.inSettings = false;
        UIManager.instance.inCredits = false;
        
    }

    public void MainMenuButton()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        SceneManager.LoadScene(0);
        gameManager.instance.stateUnpaused();
    }
    
    public void Credits()
    {
        
        AudioManager.instance.PlaySFX("ButtonPress");
        if (!UIManager.instance.inCredits)
        {
            UIManager.instance.creditScene.SetActive(true);
            UIManager.instance.backButton.SetActive(true);            
            UIManager.instance.ToggleButtons(UIManager.instance.mainmenuButtons);
            UIManager.instance.ActivateText();
            UIManager.instance.inCredits = true;
        }
    }

   
}
