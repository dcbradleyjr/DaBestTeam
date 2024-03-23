using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPocket : MonoBehaviour
{
    public int currency;
    int HPOriginal;
    int StaminaOriginal;
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;
    public Weapon meleeWeapon;

    Color StaminaColorOrig;
    Color HealthColorOrig;

    [Header("--Stats--")]
    [Range(1, 50)][SerializeField] int HP;
    [Range(0, 250)][SerializeField] int Stamina;
    [Range(1, 3)][SerializeField] int staminaDrain;


/*    [Header("--UI--")]
    public UnityEngine.UI.Image HealthBar;
    public GameObject PlayerUI;*/

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables if needed
        currency = gameManager.instance.GetCurrencyBalance();
        StaminaColorOrig = gameManager.instance.playerStaminaBar.color;
        HealthColorOrig = gameManager.instance.playerHPBar.color;

        HPOriginal = HP;
        StaminaOriginal = Stamina;
    }

    // Update is called once per frame
    void Update()
    {
        // Implement logic for updating these variables if needed
    }


    public void healHP(int amount)
    {
        if (HP + amount > HPOriginal)
        {
            HP = HPOriginal;
        }
        else
        {
            HP += amount;
        }
        updateUI();

    }


    void updateUI()
    {
        updateHealthUI();
        updateStaminaUI();
    }
    void updateStaminaUI()
    {
        /*float newAmount = (float)Stamina / StaminaOriginal;
        gameManager.instance.playerStaminaBar.fillAmount = newAmount;

        if (canSprint)
        {
            if (newAmount > 0.4f)
                gameManager.instance.playerStaminaBar.color = StaminaColorOrig;
            else
                gameManager.instance.playerStaminaBar.color = new Color(1f, 0.5f, 0f);
        }
        else
            gameManager.instance.playerStaminaBar.color = Color.red;*/
    }

    void updateHealthUI()
    {
        /*
        float newAmount = (float)HP / HPOriginal;
        gameManager.instance.playerHPBar.fillAmount = newAmount;

        if (newAmount >= 0.6f)
            gameManager.instance.playerHPBar.color = HealthColorOrig;
        else if (newAmount < 0.6f && newAmount > 0.3f)
            gameManager.instance.playerHPBar.color = new Color(1f, 0.5f, 0f);
        else
            gameManager.instance.playerHPBar.color = Color.red;*/
    }
    void sprint()
    {/*
        if (!canSprint)
            playerSpeed = playerSpeedOriginal;

        if (canSprint)
        {
            if (Input.GetButtonDown("Sprint"))
            {
                playerSpeed *= sprintMod;
                isSprinting = true;
                StartCoroutine(StaminaDrainCoroutine());
            }
            else if (Input.GetButtonUp("Sprint"))
            {
                playerSpeed /= sprintMod;
                isSprinting = false;
                StopCoroutine(StaminaDrainCoroutine());
                StartCoroutine(StaminaReplenishCoroutine());
            }
        }
        else if (isSprinting && Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            StopCoroutine(StaminaDrainCoroutine());
            StartCoroutine(StaminaReplenishCoroutine());
        }*/
    }
}

// Define a Weapon class if you haven't already
public class Weapon
{
    public string name;
    public int damage;
    // You can add more properties like range, attack speed, etc. if needed
}