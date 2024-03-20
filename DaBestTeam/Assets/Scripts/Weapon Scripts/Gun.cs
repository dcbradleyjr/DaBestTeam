using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public GameObject bulletPrefab;
    public GameObject muzzleFlash;
    public GameObject ejectBullet;
    public List<string> shotAudio;
    public List<string> reloadAudio;
    public Transform shootPoint;
    public Transform ejectPoint;
    public int damage;
    public int ammo;
    public int clipSizeMax;
    public float fireRate;
    public float reloadRate;
    public float bulletDistance;

    public bool isAuto;
}
