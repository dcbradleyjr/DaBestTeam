using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieParts : MonoBehaviour, IDamage
{
    [SerializeField] ZombieAI parentZombie;

    public void takeDamage(int amount, bool headshot)
    {
        parentZombie.takeDamage(amount, headshot);
    }
}
