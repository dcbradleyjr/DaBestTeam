using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SaveLevel();
        UpgradeSkills Upgrades = GameObject.FindWithTag("UpgradeSkills")?.GetComponent<UpgradeSkills>();
        if (Upgrades)
            Upgrades.SaveUpgradeState();
    }

    public void LoadGame()
    {
        WeaponSlotManager.instance.LoadWeapons();
        gameManager.instance.LoadCurrency();
        UpgradeSkills Upgrades = GameObject.FindWithTag("UpgradeSkills")?.GetComponent<UpgradeSkills>();
        if (Upgrades)
            Upgrades.LoadUpgradeState();
    }

    public void ResetStats()
    {
        PlayerPrefs.SetInt("ResetPlayer", 1);
        PlayerPrefs.SetInt("ResetUpgrades", 1);
        PlayerPrefs.SetInt("SavedSceneIndex", 1);
    }

    public void SaveLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        PlayerPrefs.SetInt("SavedSceneIndex", currentSceneIndex + 1);
        PlayerPrefs.Save();
    }
}
