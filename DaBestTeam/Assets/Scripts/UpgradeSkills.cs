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

    [Header("--HP--")]
    [SerializeField] GameObject HPlevel1;
    [SerializeField] GameObject HPlevel2;
    [SerializeField] GameObject HPlevel3;
    [SerializeField] GameObject HPlevel4;
    [SerializeField] GameObject HPlevel5;
    [Header("--Stamina--")]
    [SerializeField] GameObject Staminalevel1;
    [SerializeField] GameObject Staminalevel2;
    [SerializeField] GameObject Staminalevel3;
    [SerializeField] GameObject Staminalevel4;
    [SerializeField] GameObject Staminalevel5;
    [Header("--Speed--")]
    [SerializeField] GameObject Speedlevel1;
    [SerializeField] GameObject Speedlevel2;
    [SerializeField] GameObject Speedlevel3;
    [SerializeField] GameObject Speedlevel4;
    [SerializeField] GameObject Speedlevel5;

    public void Start()
    {
        
    }
   public void interact()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        UIManager.instance.SkillsMenu.SetActive(true);
        gameManager.instance.statePaused();
        UIManager.instance.menuActive = UIManager.instance.SkillsMenu;
        UIManager.instance.menuActive.SetActive(true);
    }

   public void HPUpgrade()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        int currentCurrency = gameManager.instance.GetCurrencyBalance();

        if (!level1HPAcquired && currentCurrency >= level1Cost)
        {
            Debug.Log("1");
            thirdPersonController.IncreaseHPMax(10);
            gameManager.instance.SpendCurrency(level1Cost);
            level1HPAcquired = true;
            HPlevel1.SetActive(true);
            return;
        }
        if (!level2HPAcquired && currentCurrency >= level2Cost)
        {
            Debug.Log("2");
            thirdPersonController.IncreaseHPMax(20);
            gameManager.instance.SpendCurrency(level2Cost);
            level2HPAcquired = true;
            HPlevel2.SetActive(true);
            return;
        }
        if (!level3HPAcquired && currentCurrency >= level3Cost)
        {
            Debug.Log("3");
            thirdPersonController.IncreaseHPMax(30);
            gameManager.instance.SpendCurrency(level3Cost);
            level3HPAcquired = true;
            HPlevel3.SetActive(true);
            return; 
        }
        if (!level4HPAcquired && currentCurrency >= level4Cost)
        {
            Debug.Log("4");
            thirdPersonController.IncreaseHPMax(40);
            gameManager.instance.SpendCurrency(level4Cost);
            level4HPAcquired = true;
            HPlevel4.SetActive(true);
            return;
        }
        if (!level5HPAcquired && currentCurrency >= level5Cost)
        {
            Debug.Log("5");
            thirdPersonController.IncreaseHPMax(50);
            gameManager.instance.SpendCurrency(level5Cost);
            level5HPAcquired = true;
            HPlevel5.SetActive(true);
            return;
        }

    }
   public void StaminaUpgrade()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        int currentCurrency = gameManager.instance.GetCurrencyBalance();

        if (!level1StaminaAcquired && currentCurrency >= level1Cost)
        {
            Debug.Log("1");
            thirdPersonController.IncreaseStamina(10);
            gameManager.instance.SpendCurrency(level1Cost);
            level1StaminaAcquired = true;
            Staminalevel1.SetActive(true);
            return;
        }
        if (!level2StaminaAcquired && currentCurrency >= level2Cost)
        {
            Debug.Log("2");
            thirdPersonController.IncreaseStamina(20);
            gameManager.instance.SpendCurrency(level2Cost);
            level2StaminaAcquired = true;
            Staminalevel2.SetActive(true);
            return;
        }
        if (!level3StaminaAcquired && currentCurrency >= level3Cost)
        {
            Debug.Log("3");
            thirdPersonController.IncreaseStamina(30);
            gameManager.instance.SpendCurrency(level3Cost);
            level3StaminaAcquired = true;
            Staminalevel3.SetActive(true);
            return;
        }
        
        if (!level4StaminaAcquired && currentCurrency >= level4Cost)
        {
            Debug.Log("4");
            thirdPersonController.IncreaseStamina(40);
            gameManager.instance.SpendCurrency(level5Cost);
            level4StaminaAcquired = true;
            Staminalevel4.SetActive(true);
            return;
        }
        if (!level5StaminaAcquired && currentCurrency >= level5Cost)
        {
            Debug.Log("5");
            thirdPersonController.IncreaseStamina(50);
            gameManager.instance.SpendCurrency(level5Cost);
            level5StaminaAcquired = true;
            Staminalevel5.SetActive(true);
            return;
        }
    }
    public void SpeedUpgrade()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        int currentCurrency = gameManager.instance.GetCurrencyBalance();
        if (!level1SpeedAcquired && currentCurrency >= level1Cost)
        {
            Debug.Log("1");
            thirdPersonController.IncreaseSpeed(10);
            gameManager.instance.SpendCurrency(level1Cost);
            level1SpeedAcquired = true;
            Speedlevel1.SetActive(true);
            return;
        }
        if (!level2SpeedAcquired && currentCurrency >= level2Cost)
        {
            Debug.Log("2");
            thirdPersonController.IncreaseSpeed(20);
            gameManager.instance.SpendCurrency(level2Cost);
            level2SpeedAcquired = true;
            Speedlevel2.SetActive(true);
            return;
        }
        if (!level3SpeedAcquired && currentCurrency >= level3Cost)
        {
            Debug.Log("3");
            thirdPersonController.IncreaseSpeed(30);
            gameManager.instance.SpendCurrency(level3Cost);
            level3SpeedAcquired = true;
            Speedlevel3.SetActive(true);
            return;
        }        
        if (!level4SpeedAcquired && currentCurrency >= level4Cost)
        {
            Debug.Log("4");
            thirdPersonController.IncreaseSpeed(40);
            gameManager.instance.SpendCurrency(level4Cost);
            level4SpeedAcquired = true;
            Speedlevel4.SetActive(true);
            return;
        }
        if (!level5SpeedAcquired && currentCurrency >= level5Cost)
        {
            Debug.Log("5");
            thirdPersonController.IncreaseSpeed(50);
            gameManager.instance.SpendCurrency(level5Cost);
            level5SpeedAcquired = true;
            Speedlevel5.SetActive(true);
            return;
        }
    }
   public void back()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        UIManager.instance.SkillsMenu.SetActive(false);
        gameManager.instance.stateUnpaused();
    }
}

