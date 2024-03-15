using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeSkills : MonoBehaviour, IInteract
{
   public ThirdPersonController thirdPersonController;

   public string interactPrompt => ("Skills Crate!");

   public int level1Cost;
   public int level2Cost;
   public int level3Cost;
   public int level4Cost;
   public int level5Cost;

   private bool level1HPAcquired = false;
   private bool level2HPAcquired = false;
   private bool level3HPAcquired = false;
   private bool level4HPAcquired = false;
   private bool level5HPAcquired = false;

   private bool level1StaminaAcquired = false;
   private bool level2StaminaAcquired = false;
   private bool level3StaminaAcquired = false;
   private bool level4StaminaAcquired = false;
   private bool level5StaminaAcquired = false;

   /*private bool level1DamageAcquired = false;
   private bool level2DamageAcquired = false;
   private bool level3DamageAcquired = false;
   private bool level4DamageAcquired = false;
   private bool level5DamageAcquired = false;*/

   private bool level1SpeedAcquired = false;
   private bool level2SpeedAcquired = false;
   private bool level3SpeedAcquired = false;
   private bool level4SpeedAcquired = false;
   private bool level5SpeedAcquired = false;

   public void Start()
    {
        
    }
   public void interact()
    {
        gameManager.instance.SkillsMenu.SetActive(true);
        gameManager.instance.statePaused();
        gameManager.instance.menuActive = gameManager.instance.SkillsMenu;
        gameManager.instance.menuActive.SetActive(true);
    }

   public void HPUpgrade()
    {
        int currentCurrency = gameManager.instance.GetCurrencyBalance();

        if (!level1HPAcquired && currentCurrency >= level1Cost)
        {
            Debug.Log("1");
            thirdPersonController.IncreaseHPMax(10);
            gameManager.instance.SpendCurrency(level1Cost);
            level1HPAcquired = true;
            return;
        }
        if (!level2HPAcquired && currentCurrency >= level2Cost)
        {
            Debug.Log("2");
            thirdPersonController.IncreaseHPMax(20);
            gameManager.instance.SpendCurrency(level2Cost);
            level2HPAcquired = true;
            return;
        }
        if (!level3HPAcquired && currentCurrency >= level3Cost)
        {
            Debug.Log("3");
            thirdPersonController.IncreaseHPMax(30);
            gameManager.instance.SpendCurrency(level3Cost);
            level3HPAcquired = true;
            return; 
        }
        if (!level4HPAcquired && currentCurrency >= level4Cost)
        {
            Debug.Log("4");
            thirdPersonController.IncreaseHPMax(40);
            gameManager.instance.SpendCurrency(level4Cost);
            level4HPAcquired = true;
            return;
        }
        if (!level5HPAcquired && currentCurrency >= level5Cost)
        {
            Debug.Log("5");
            thirdPersonController.IncreaseHPMax(50);
            gameManager.instance.SpendCurrency(level5Cost);
            level5HPAcquired = true;
            return;
        }

    }
   public void StaminaUpgrade()
    {
        int currentCurrency = gameManager.instance.GetCurrencyBalance();

        if (!level5StaminaAcquired && currentCurrency >= level5Cost)
        {
            Debug.Log("5");
            thirdPersonController.IncreaseStamina(50);
            gameManager.instance.SpendCurrency(level5Cost);
            level5StaminaAcquired = true;
            return;
        }
        if (!level4StaminaAcquired && currentCurrency >= level4Cost)
        {
            Debug.Log("4");
            thirdPersonController.IncreaseStamina(40);
            gameManager.instance.SpendCurrency(level5Cost);
            level4StaminaAcquired = true;
            return;
        }
        if (!level3StaminaAcquired && currentCurrency >= level3Cost)
        {
            Debug.Log("3");
            thirdPersonController.IncreaseStamina(30);
            gameManager.instance.SpendCurrency(level3Cost);
            level3StaminaAcquired = true;
            return;
        }
        if (!level2StaminaAcquired && currentCurrency >= level2Cost)
        {
            Debug.Log("2");
            thirdPersonController.IncreaseStamina(20);
            gameManager.instance.SpendCurrency(level2Cost);
            level2StaminaAcquired = true;
            return;
        }
        if (!level1StaminaAcquired && currentCurrency >= level1Cost)
        {
            Debug.Log("1");
            thirdPersonController.IncreaseStamina(10);
            gameManager.instance.SpendCurrency(level1Cost);
            level1StaminaAcquired = true;
            return;
        }
    }
    public void SpeedUpgrade()
    {
        int currentCurrency = gameManager.instance.GetCurrencyBalance();
        if (!level5SpeedAcquired && currentCurrency >= level5Cost)
        {
            Debug.Log("5");
            thirdPersonController.IncreaseSpeed(50);
            gameManager.instance.SpendCurrency(level5Cost);
            level5SpeedAcquired = true;
            return;
        }
        if (!level4SpeedAcquired && currentCurrency >= level4Cost)
        {
            Debug.Log("4");
            thirdPersonController.IncreaseSpeed(40);
            gameManager.instance.SpendCurrency(level4Cost);
            level4SpeedAcquired = true;
            return;
        }
        if (!level3SpeedAcquired && currentCurrency >= level3Cost)
        {
            Debug.Log("3");
            thirdPersonController.IncreaseSpeed(30);
            gameManager.instance.SpendCurrency(level3Cost);
            level3SpeedAcquired = true;
            return;
        }
        if (!level2SpeedAcquired && currentCurrency >= level2Cost)
        {
            Debug.Log("2");
            thirdPersonController.IncreaseSpeed(20);
            gameManager.instance.SpendCurrency(level2Cost);
            level2SpeedAcquired = true;
            return;
        }
        if (!level1SpeedAcquired && currentCurrency >= level1Cost)
        {
            Debug.Log("1");
            thirdPersonController.IncreaseSpeed(10);
            gameManager.instance.SpendCurrency(level1Cost);
            level1SpeedAcquired = true;
            return;
        }
    }
   public void back()
    {
        gameManager.instance.SkillsMenu.SetActive(false);
        gameManager.instance.stateUnpaused();
    }
}

