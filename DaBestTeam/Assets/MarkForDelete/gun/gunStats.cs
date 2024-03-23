using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    public GameObject bullet;
    public string myName;
    public float shootRate;
    public float reloadRate;
    public int ammoCur;
    public int ammoMax;
    public bool isAuto;

    public GameObject model;
    public ParticleSystem hitEffect;
    public AudioClip shotSound;
    [Range(0, 1)] public float shootSoundVol;
}
