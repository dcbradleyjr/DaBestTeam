using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    private bool inSettings;

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
    public void respawn()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        gameManager.instance.stateUnpaused();
        gameManager.instance.player.Respawn();
    }             
    public void back()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        if (inSettings)
        {
            UIManager.instance.ToggleButtons(UIManager.instance.PausemenuButtons);
            UIManager.instance.ToggleButtons(UIManager.instance.PausesettingsMenuButtons);
            inSettings = false;
        }
        UIManager.instance.pausebackButton.SetActive(false);        
    }
    public void Settings()    
    {        
        inSettings = true;
        AudioManager.instance.PlaySFX("ButtonPress");        
        UIManager.instance.ToggleButtons(UIManager.instance.PausemenuButtons);
        UIManager.instance.pausebackButton.SetActive(true);
        UIManager.instance.ToggleButtons(UIManager.instance.PausesettingsMenuButtons);
    }
    public void MainMenuButton()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        gameManager.instance.stateUnpaused();
        SceneManager.LoadScene(0);       
    }   
    
}
