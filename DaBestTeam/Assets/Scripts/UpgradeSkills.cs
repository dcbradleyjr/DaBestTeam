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
    public int currencyCost = 500; // Cost of upgrading a skill
    public SkillType skillType; // Type of skill to upgrade

    // Implement your currency system here
    public int currencyAmount = 0; // Current amount of currency

    public void interact()
    {
        if (currencyAmount >= currencyCost)
        {
            UpgradeSkill();
            currencyAmount -= currencyCost;
        }
        else
        {
            Debug.Log("Not enough currency to upgrade the skill.");
        }
    }

    void UpgradeSkill()
    {
        // Implement logic to upgrade the skill based on the selected enum value
        switch (skillType)
        {
            case SkillType.Stamina:
                // Upgrade stamina
                Debug.Log("Stamina upgraded!");
                break;
            case SkillType.HP:
                // Upgrade HP
                Debug.Log("HP upgraded!");
                break;
            case SkillType.Speed:
                // Upgrade speed
                Debug.Log("Speed upgraded!");
                break;
            case SkillType.Damage:
                // Upgrade damage
                Debug.Log("Damage upgraded!");
                break;
            default:
                Debug.LogError("Unknown skill type.");
                break;
        }
    }
}

