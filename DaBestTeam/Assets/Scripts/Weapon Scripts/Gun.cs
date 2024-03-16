using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public GameObject bulletPrefab;
    public GameObject muzzleFlash;
    public Transform shootPoint;
    public int damage;
    public int ammo;
    public int clipSizeMax;
    public float fireRate;
    public float reloadRate;
    public float bulletDistance;

    public bool isAuto;
}
