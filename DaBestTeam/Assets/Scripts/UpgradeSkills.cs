using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Stamina,
    HP,
    Speed,
    Damage
}

public class UpgradeSkills : MonoBehaviour, IInteract
{
    public string interactPrompt => ("Skills Crate!");

    
    public int currentCurrency;
    public int level1Cost;
    public int level2Cost;
    public int level3Cost;
    public int level4Cost;
    public int level5Cost;

    public void Start()
    {
        
    }
    public void interact()
    {
        gameManager.instance.SkillsMenu.SetActive(true);
        gameManager.instance.statePaused();
    }

    public void HPUpgrade()
    {
        int currentCurrency = gameManager.instance.GetCurrencyBalance();
        if (currentCurrency >= level1Cost)
        {
           
        }
        else if (currentCurrency >= level2Cost)
        {

        }
        else if(currentCurrency >= level3Cost)
        {

        }
        else if(currentCurrency >= level4Cost)
        {

        }
        else if(currentCurrency >= level5Cost)
        {

        }
    }
    public void TemplateUpgrade()
    {
        int currentCurrency = gameManager.instance.GetCurrencyBalance();
        if (currentCurrency >= level1Cost)
        {

        }
        else if (currentCurrency >= level2Cost)
        {

        }
        else if (currentCurrency >= level3Cost)
        {

        }
        else if (currentCurrency >= level4Cost)
        {

        }
        else if (currentCurrency >= level5Cost)
        {

        }
    }

    public void back()
    {
        gameManager.instance.SkillsMenu.SetActive(false);
        gameManager.instance.stateUnpaused();
    }
}

