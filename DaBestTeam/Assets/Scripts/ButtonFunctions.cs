using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{

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
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        Debug.Log("Here");
        AudioManager.instance.PlaySFX("ButtonPress");
        gameManager.instance.SettingsMenu.gameObject.SetActive(true);
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            gameManager.instance.MainMenu.gameObject.SetActive(false);
        }
        else 
        {
            gameManager.instance.menuPause.gameObject.SetActive(false);
            gameManager.instance.SettingsMenu.gameObject.SetActive(true);
        }
    }


    public void MainMenuSettingsBack()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            gameManager.instance.MainMenu.gameObject.SetActive(true);
            gameManager.instance.SettingsMenu.gameObject.SetActive(false);
        }
        else
        {
            gameManager.instance.SettingsMenu.gameObject.SetActive(false);
            gameManager.instance.menuPause.gameObject.SetActive(true);
        }
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
    
}
