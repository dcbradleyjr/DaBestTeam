using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamage, IHealth, IPushBack
{

    [Range(1, 50)][SerializeField] int HP;
    int HPOriginal;

    public UnityEngine.UI.Image HealthBar;
    public GameObject EnemyUI;

    // Start is called before the first frame update
    void Start()
    {
        HPOriginal = HP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int amount)
    {

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
/*        updateHealthUI();
        StartCoroutine(flashHeal());*/
    }

    public void pushBackDir(Vector3 dir)
    {
        
    }
}
