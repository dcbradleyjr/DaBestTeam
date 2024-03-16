using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeSkills : MonoBehaviour, IInteract
{    
    public string interactPrompt => ("Skills Crate!");
   
    public int level1Cost;
    public int level2Cost;
    public int level3Cost;
    public int level4Cost;
    public int level5Cost;
   
    [Header("--HP--")]
    [SerializeField] GameObject HPlevel1;
    [SerializeField] GameObject HPlevel2;
    [SerializeField] GameObject HPlevel3;
    [SerializeField] GameObject HPlevel4;
    [SerializeField] GameObject HPlevel5;

    public bool level1HPAcquired = false;
    public bool level2HPAcquired = false;
    public bool level3HPAcquired = false;
    public bool level4HPAcquired = false;
    public bool level5HPAcquired = false;

    [Range(0, 100)] public int level1HPAmount;
    [Range(0, 100)] public int level2HPAmount;
    [Range(0, 100)] public int level3HPAmount;
    [Range(0, 100)] public int level4HPAmount;
    [Range(0, 100)] public int level5HPAmount;

    [Header("--Stamina--")]
    [SerializeField] GameObject Staminalevel1;
    [SerializeField] GameObject Staminalevel2;
    [SerializeField] GameObject Staminalevel3;
    [SerializeField] GameObject Staminalevel4;
    [SerializeField] GameObject Staminalevel5;

    public bool level1StaminaAcquired = false;
    public bool level2StaminaAcquired = false;
    public bool level3StaminaAcquired = false;
    public bool level4StaminaAcquired = false;
    public bool level5StaminaAcquired = false;

    [Range(0, 100)] public int level1StaminaAmount;
    [Range(0, 100)] public int level2StaminaAmount;
    [Range(0, 100)] public int level3StaminaAmount;
    [Range(0, 100)] public int level4StaminaAmount;
    [Range(0, 100)] public int level5StaminaAmount;

    [Header("--Speed--")]
    [SerializeField] GameObject Speedlevel1;
    [SerializeField] GameObject Speedlevel2;
    [SerializeField] GameObject Speedlevel3;
    [SerializeField] GameObject Speedlevel4;
    [SerializeField] GameObject Speedlevel5;

    public bool level1SpeedAcquired = false;
    public bool level2SpeedAcquired = false;
    public bool level3SpeedAcquired = false;
    public bool level4SpeedAcquired = false;
    public bool level5SpeedAcquired = false;

    [Range(0, 100)] public int level1SpeedAmount;
    [Range(0, 100)] public int level2SpeedAmount;
    [Range(0, 100)] public int level3SpeedAmount;
    [Range(0, 100)] public int level4SpeedAmount;
    [Range(0, 100)] public int level5SpeedAmount;

    [Header("--Damage--")]
    [SerializeField] GameObject Damagelevel1;
    [SerializeField] GameObject Damagelevel2;
    [SerializeField] GameObject Damagelevel3;
    [SerializeField] GameObject Damagelevel4;
    [SerializeField] GameObject Damagelevel5;

    public bool level1DamageAcquired = false;
    public bool level2DamageAcquired = false;
    public bool level3DamageAcquired = false;
    public bool level4DamageAcquired = false;
    public bool level5DamageAcquired = false;

    [Range(0, 100)] public int level1DamageAmount;
    [Range(0, 100)] public int level2DamageAmount;
    [Range(0, 100)] public int level3DamageAmount;
    [Range(0, 100)] public int level4DamageAmount;
    [Range(0, 100)] public int level5DamageAmount;

    public void Start()
    {
        LoadUpgradeState();
        EnableUpgradeGameObjects();
    }
    public void interact()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        UIManager.instance.SkillsMenu.SetActive(true);
        gameManager.instance.statePaused();
        UIManager.instance.menuActive = UIManager.instance.SkillsMenu;
        UIManager.instance.menuActive.SetActive(true);        
    }
    public void Update()
    {
        
    }
    public static bool GetBool(string key, bool defaultValue = false)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
    }
    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void HPUpgrade()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        int currentCurrency = gameManager.instance.GetCurrencyBalance();

        if (!level1HPAcquired && currentCurrency >= level1Cost)
        {
            Debug.Log("1");
            gameManager.instance.player.IncreaseHPMax(level1HPAmount);
            gameManager.instance.SpendCurrency(level1Cost);
            level1HPAcquired = true;
            HPlevel1.SetActive(true);
            return;
        }
        if (!level2HPAcquired && currentCurrency >= level2Cost)
        {
            Debug.Log("2");
            gameManager.instance.player.IncreaseHPMax(level2HPAmount);
            gameManager.instance.SpendCurrency(level2Cost);
            level2HPAcquired = true;
            HPlevel2.SetActive(true);
            return;
        }
        if (!level3HPAcquired && currentCurrency >= level3Cost)
        {
            Debug.Log("3");
            gameManager.instance.player.IncreaseHPMax(level3HPAmount);
            gameManager.instance.SpendCurrency(level3Cost);
            level3HPAcquired = true;
            HPlevel3.SetActive(true);
            return; 
        }
        if (!level4HPAcquired && currentCurrency >= level4Cost)
        {
            Debug.Log("4");
            gameManager.instance.player.IncreaseHPMax(level4HPAmount);
            gameManager.instance.SpendCurrency(level4Cost);
            level4HPAcquired = true;
            HPlevel4.SetActive(true);
            return;
        }
        if (!level5HPAcquired && currentCurrency >= level5Cost)
        {
            Debug.Log("5");
            gameManager.instance.player.IncreaseHPMax(level5HPAmount);
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
            gameManager.instance.player.IncreaseStamina(level1StaminaAmount);
            gameManager.instance.SpendCurrency(level1Cost);
            level1StaminaAcquired = true;
            Staminalevel1.SetActive(true);
            return;
        }
        if (!level2StaminaAcquired && currentCurrency >= level2Cost)
        {
            Debug.Log("2");
            gameManager.instance.player.IncreaseStamina(level2StaminaAmount);
            gameManager.instance.SpendCurrency(level2Cost);
            level2StaminaAcquired = true;
            Staminalevel2.SetActive(true);
            return;
        }
        if (!level3StaminaAcquired && currentCurrency >= level3Cost)
        {
            Debug.Log("3");
            gameManager.instance.player.IncreaseStamina(level3StaminaAmount);
            gameManager.instance.SpendCurrency(level3Cost);
            level3StaminaAcquired = true;
            Staminalevel3.SetActive(true);
            return;
        }
        
        if (!level4StaminaAcquired && currentCurrency >= level4Cost)
        {
            Debug.Log("4");
            gameManager.instance.player.IncreaseStamina(level4StaminaAmount);
            gameManager.instance.SpendCurrency(level5Cost);
            level4StaminaAcquired = true;
            Staminalevel4.SetActive(true);
            return;
        }
        if (!level5StaminaAcquired && currentCurrency >= level5Cost)
        {
            Debug.Log("5");
            gameManager.instance.player.IncreaseStamina(level5StaminaAmount);
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
            gameManager.instance.player.IncreaseSpeed(level1SpeedAmount);
            gameManager.instance.SpendCurrency(level1Cost);
            level1SpeedAcquired = true;
            Speedlevel1.SetActive(true);
            return;
        }
        if (!level2SpeedAcquired && currentCurrency >= level2Cost)
        {
            Debug.Log("2");
            gameManager.instance.player.IncreaseSpeed(level2SpeedAmount);
            gameManager.instance.SpendCurrency(level2Cost);
            level2SpeedAcquired = true;
            Speedlevel2.SetActive(true);
            return;
        }
        if (!level3SpeedAcquired && currentCurrency >= level3Cost)
        {
            Debug.Log("3");
            gameManager.instance.player.IncreaseSpeed(level3SpeedAmount);
            gameManager.instance.SpendCurrency(level3Cost);
            level3SpeedAcquired = true;
            Speedlevel3.SetActive(true);
            return;
        }        
        if (!level4SpeedAcquired && currentCurrency >= level4Cost)
        {
            Debug.Log("4");
            gameManager.instance.player.IncreaseSpeed(level4SpeedAmount);
            gameManager.instance.SpendCurrency(level4Cost);
            level4SpeedAcquired = true;
            Speedlevel4.SetActive(true);
            return;
        }
        if (!level5SpeedAcquired && currentCurrency >= level5Cost)
        {
            Debug.Log("5");
            gameManager.instance.player.IncreaseSpeed(level5SpeedAmount);
            gameManager.instance.SpendCurrency(level5Cost);
            level5SpeedAcquired = true;
            Speedlevel5.SetActive(true);
            return;
        }
    }
    public void DamageUpgrade()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        int currentCurrency = gameManager.instance.GetCurrencyBalance();
        if (!level1DamageAcquired && currentCurrency >= level1Cost)
        {
            Debug.Log("1");
            WeaponSlotManager.instance.Melee.IncreaseDamage(level1DamageAmount);
            gameManager.instance.SpendCurrency(level1Cost);
            level1DamageAcquired = true;
            Damagelevel1.SetActive(true);
            return;
        }
        if (!level2DamageAcquired && currentCurrency >= level2Cost)
        {
            Debug.Log("2");
            WeaponSlotManager.instance.Melee.IncreaseDamage(level2DamageAmount);
            gameManager.instance.SpendCurrency(level2Cost);
            level2DamageAcquired = true;
            Damagelevel2.SetActive(true);
            return;
        }
        if (!level3DamageAcquired && currentCurrency >= level3Cost)
        {
            Debug.Log("3");
            WeaponSlotManager.instance.Melee.IncreaseDamage(level3DamageAmount);
            gameManager.instance.SpendCurrency(level3Cost);
            level3DamageAcquired = true;
            Damagelevel3.SetActive(true);
            return;
        }
        if (!level4DamageAcquired && currentCurrency >= level4Cost)
        {
            Debug.Log("4");
            WeaponSlotManager.instance.Melee.IncreaseDamage(level4DamageAmount);
            gameManager.instance.SpendCurrency(level4Cost);
            level4DamageAcquired = true;
            Damagelevel4.SetActive(true);
            return;
        }
        if (!level5DamageAcquired && currentCurrency >= level5Cost)
        {
            Debug.Log("5");
            WeaponSlotManager.instance.Melee.IncreaseDamage(level5DamageAmount);
            gameManager.instance.SpendCurrency(level5Cost);
            level5DamageAcquired = true;
            Damagelevel5.SetActive(true);
            return;
        }
    }
    public void back()
    {
        AudioManager.instance.PlaySFX("ButtonPress");
        UIManager.instance.SkillsMenu.SetActive(false);
        gameManager.instance.stateUnpaused();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SaveUpgradeState();
    }
    public void SaveUpgradeState()
    {
        SetBool("Level1HPAcquired", level1HPAcquired);
        SetBool("Level2HPAcquired", level2HPAcquired);
        SetBool("Level3HPAcquired", level3HPAcquired);
        SetBool("Level4HPAcquired", level4HPAcquired);
        SetBool("Level5HPAcquired", level5HPAcquired);
       
        SetBool("Level1StaminaAcquired", level1StaminaAcquired);
        SetBool("Level2StaminaAcquired", level2StaminaAcquired);
        SetBool("Level3StaminaAcquired", level3StaminaAcquired);
        SetBool("Level4StaminaAcquired", level4StaminaAcquired);
        SetBool("Level5StaminaAcquired", level5StaminaAcquired);
       
        SetBool("Level1SpeedAcquired", level1SpeedAcquired);
        SetBool("Level2SpeedAcquired", level2SpeedAcquired);
        SetBool("Level3SpeedAcquired", level3SpeedAcquired);
        SetBool("Level4SpeedAcquired", level4SpeedAcquired);
        SetBool("Level5SpeedAcquired", level5SpeedAcquired);

        SetBool("Level1DamageAcquired", level1DamageAcquired);
        SetBool("Level2DamageAcquired", level2DamageAcquired);
        SetBool("Level3DamageAcquired", level3DamageAcquired);
        SetBool("Level4DamageAcquired", level4DamageAcquired);
        SetBool("Level5DamageAcquired", level5DamageAcquired);


        PlayerPrefs.Save();
    }
    public void LoadUpgradeState()
    {
        level1HPAcquired = GetBool("Level1HPAcquired");
        level2HPAcquired = GetBool("Level2HPAcquired");
        level3HPAcquired = GetBool("Level3HPAcquired");
        level4HPAcquired = GetBool("Level4HPAcquired");
        level5HPAcquired = GetBool("Level5HPAcquired");

        level1StaminaAcquired = GetBool("Level1StaminaAcquired");
        level2StaminaAcquired = GetBool("Level2StaminaAcquired");
        level3StaminaAcquired = GetBool("Level3StaminaAcquired");
        level4StaminaAcquired = GetBool("Level4StaminaAcquired");
        level5StaminaAcquired = GetBool("Level5StaminaAcquired");

        level1SpeedAcquired = GetBool("Level1SpeedAcquired");
        level2SpeedAcquired = GetBool("Level2SpeedAcquired");
        level3SpeedAcquired = GetBool("Level3SpeedAcquired");
        level4SpeedAcquired = GetBool("Level4SpeedAcquired");
        level5SpeedAcquired = GetBool("Level5SpeedAcquired");

        level1DamageAcquired = GetBool("Level1DamageAcquired");
        level2DamageAcquired = GetBool("Level2DamageAcquired");
        level3DamageAcquired = GetBool("Level3DamageAcquired");
        level4DamageAcquired = GetBool("Level4DamageAcquired");
        level5DamageAcquired = GetBool("Level5DamageAcquired");
    }
    public void EnableUpgradeGameObjects()
    {
        EnableHPUpgradeGameObjects();
        EnableStaminaUpgradeGameObjects();
        EnableSpeedUpgradeGameObjects();
        EnableDamageUpgradeGameObjects();
    }
    void EnableHPUpgradeGameObjects()
    {
        if (level1HPAcquired)
            HPlevel1.SetActive(true);
        if (level2HPAcquired)
            HPlevel2.SetActive(true);
        if(level3HPAcquired)
            HPlevel3.SetActive(true);
        if(level4HPAcquired)
            HPlevel4.SetActive(true);
        if(level5HPAcquired)
            HPlevel5.SetActive(true);
    }
    void EnableStaminaUpgradeGameObjects()
    {
        if (level1StaminaAcquired)
            Staminalevel1.SetActive(true);
        if (level2StaminaAcquired)
            Staminalevel2.SetActive(true);
        if(level3StaminaAcquired)
            Staminalevel3.SetActive(true);
        if(level4StaminaAcquired)
            Staminalevel4.SetActive(true);
        if(level5StaminaAcquired)
            Staminalevel5.SetActive(true);
    }
    void EnableSpeedUpgradeGameObjects()
    {
        if (level1SpeedAcquired)
            Speedlevel1.SetActive(true);
        if (level2SpeedAcquired)
            Speedlevel2.SetActive(true);
        if (level3SpeedAcquired)
            Speedlevel3.SetActive(true);
        if(level4SpeedAcquired)
            Speedlevel4.SetActive(true);
        if(level5SpeedAcquired) 
            Speedlevel5.SetActive(true);
    }
    void EnableDamageUpgradeGameObjects()
    {
        if (level1DamageAcquired)
            Damagelevel1.SetActive(true);
        if (level2DamageAcquired)
            Damagelevel2.SetActive(true);
        if (level3DamageAcquired)
            Damagelevel3.SetActive(true);
        if (level4DamageAcquired)
            Damagelevel4.SetActive(true);
        if (level5DamageAcquired)
            Damagelevel5.SetActive(true);
    }
}

