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
        PlayerPrefs.SetInt("ResetUpgrades", 1);

        //logic in start/awake of upgrade skills
        //UIManager.instance.upgradeSkills.StatReset();
        //UIManager.instance.upgradeSkills.SaveUpgradeState();
    }
}
