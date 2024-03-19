using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void SaveGame()
    {
        WeaponSlotManager.instance.SaveWeapons();
        gameManager.instance.SaveCurrency();
    }

    public void LoadGame()
    {
        WeaponSlotManager.instance.LoadWeapons();
        gameManager.instance.LoadCurrency();
    }

    public void ResetStats()
    {
        PlayerPrefs.SetInt("ResetPlayer", 1);
        //UIManager.instance.upgradeSkills.StatReset();
        //gameManager.instance.ResetCurrency();
        //save the reset
        //UIManager.instance.upgradeSkills.SaveUpgradeState();
        //gameManager.instance.SaveCurrency();
    }
}