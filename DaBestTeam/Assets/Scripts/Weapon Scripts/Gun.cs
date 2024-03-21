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
    public Transform raycast;
    public int damage;
    public int ammo;
    public int clipSizeMax;
    public float fireRate;
    public float reloadRate;
    public float bulletDistance;
    public float maxSpreadAngle;
    public int bulletCount;
    public float recoil;

    public bool isAuto;
    public bool isShotgun;
    public bool isSniper;
}
