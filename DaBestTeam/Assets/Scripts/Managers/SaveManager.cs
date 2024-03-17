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
        UIManager.instance.upgradeSkills.SaveUpgradeState();
        gameManager.instance.SaveCurrency();
    }

    public void LoadGame()
    {
        WeaponSlotManager.instance.LoadWeapons();
        UIManager.instance.upgradeSkills.LoadUpgradeState();
        UIManager.instance.upgradeSkills.EnableUpgradeGameObjects();
        gameManager.instance.LoadCurrency();
    }

    public void ResetStats()
    {
        PlayerPrefs.SetInt("ResetPlayer", 1);
        UIManager.instance.upgradeSkills.StatReset();
        gameManager.instance.ResetCurrency();
        //save the reset
        UIManager.instance.upgradeSkills.SaveUpgradeState();
        gameManager.instance.SaveCurrency();
    }
}
