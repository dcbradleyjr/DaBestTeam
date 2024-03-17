using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSlot : MonoBehaviour
{
    [Header("---Components---")]
    [SerializeField] PlayerInput input;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Transform shootPoint;
    [SerializeField] Animator animator;

    [Header("---Pistol = 1, Gun = 3---")]
    [SerializeField] int layerIndex;

    [Header("---GunList---")]
    [SerializeField] List<GameObject> gunList;
    [SerializeField] List<Gun> gunStats;
    [SerializeField] int currentGunIndex;

    [Header("---Stats---")]
    [SerializeField] string gunName;
    [SerializeField] int damage;
    [SerializeField] int clipSize;
    [SerializeField] int clipSizeMax;
    [SerializeField] int ammo;
    [SerializeField] float reloadRate;
    [SerializeField] float fireRate;
    [SerializeField] float bulletDistance = 25f;
    Transform cameraTransform;
    InputAction shootAction;
    InputAction reloadAction;

    bool canShoot;
    public bool toggleShoot;
    [Header("---States---")]
    public bool isShooting;
    public bool isReloading;
    public bool isAuto;
    public bool isInfiniteAmmo;

    void Awake()
    {
        shootAction = input.actions["Shoot"];
        reloadAction = input.actions["Reload"];
        cameraTransform = Camera.main.transform;
        clipSize = clipSizeMax;
    }

    private void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            if (CanShootAuto())
                StartCoroutine(ShootGun()); 

            if(CanReload())
                StartCoroutine(reloadGun());

            if (!IsAnyGunActive() && currentGunIndex != -1)
            {
                ActivateGun(currentGunIndex);
            }
        }
    }

    private void OnEnable()
    {
        shootAction.performed += _ => StartShooting();
        shootAction.canceled += _ => StopShooting();
        canShoot = true;
        animator.SetLayerWeight(layerIndex, 1);
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => StartShooting();
        shootAction.canceled -= _ => StopShooting();
        canShoot = false;
        isShooting = false;
        StopAllCoroutines();
        animator.SetLayerWeight(layerIndex, 0);
    }

    public void ActivateGun(int index)
    {
        if (index >= 0 && index < gunList.Count)
        {
            DisableAllGuns(); 

            gunList[index].SetActive(true);
            currentGunIndex = index;
            TransferStats(gunStats[index]);
        }
        else
        {
            Debug.LogError("Invalid gun index!");
        }
    }

    private void TransferStats(Gun gun)
    {
        gunName = gun.gunName;
        damage = gun.damage;
        bulletPrefab = gun.bulletPrefab;
        muzzleFlash = gun.muzzleFlash;
        ammo = gun.ammo;
        clipSizeMax = gun.clipSizeMax;
        clipSize = clipSizeMax;
        fireRate = gun.fireRate;
        reloadRate = gun.reloadRate;
        bulletDistance = gun.bulletDistance;

        isAuto = gun.isAuto;
    }

    private void DisableAllGuns()
    {
        foreach (GameObject gunObject in gunList)
        {
            gunObject.SetActive(false);
        }
    }

    private void StartShooting()
    {
        toggleShoot = true;
        if(CanShoot())
        StartCoroutine(ShootGun());
    }

    private void StopShooting()
    {
        toggleShoot = false;
    }

    IEnumerator ShootGun()
    {
        isShooting = true;
        if (canShoot)
        {
            RaycastHit hit;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            GameObject flash = Instantiate(muzzleFlash, shootPoint.position, shootPoint.rotation);
            clipSize--;
            UpdateUI();
            Destroy(flash, 0.5f);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.damageAmount = damage;
            bulletController.timeToDestroy = bulletDistance;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
            }
            else
            {
                bulletController.target = cameraTransform.position + cameraTransform.forward * bulletDistance;
                bulletController.hit = false;
            }
        }
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }

    IEnumerator reloadGun()
    {
        //audio 
        isReloading = true;
        StartCoroutine(reloadingVisuals());
        yield return new WaitForSeconds(reloadRate);
        ammoDeduction();
        UpdateUI();
        isReloading = false;
    }

    void ammoDeduction()
    {
        if (isInfiniteAmmo)
        {
            clipSize = clipSizeMax;
            return;
        }
        if (ammo >= clipSizeMax)
        {
            int ammoToReload = clipSizeMax - clipSize;
            ammo -= ammoToReload;
            clipSize = clipSizeMax;
        }
        else if (ammo > 0)
        {
            clipSize = ammo;
            ammo = 0;
        }
    }

    IEnumerator reloadingVisuals()
    {
        UIManager.instance.ammoReloadDisplay.text = "Reloading";
        yield return new WaitForSeconds(reloadRate / 4);
        UIManager.instance.ammoReloadDisplay.text = "Reloading.";
        yield return new WaitForSeconds(reloadRate / 4);
        UIManager.instance.ammoReloadDisplay.text = "Reloading..";
        yield return new WaitForSeconds(reloadRate / 4);
        UIManager.instance.ammoReloadDisplay.text = "Reloading...";
        yield return new WaitForSeconds(reloadRate / 4);
        UIManager.instance.ammoReloadDisplay.text = "";
    }

    public void UpdateUI()
    {
        if (!isInfiniteAmmo)
        UIManager.instance.ammoCurrentDisplay.text = "" + clipSize + " / " + ammo;
        else
        UIManager.instance.ammoCurrentDisplay.text = "" + clipSize + " / 999";
    }

    bool CanReload()
    {
        return clipSize <= 0 && !isReloading && ammo != 0 || reloadAction.triggered && !isReloading && clipSize != clipSizeMax && ammo != 0;
    }

    bool CanShoot()
    {
        return !isShooting && canShoot && clipSize > 0 && !isReloading;
    }

    bool CanShootAuto()
    {
        return toggleShoot && isAuto && !isShooting && clipSize > 0 && !isReloading;
    }

    public bool IsAnyGunActive()
    {
        foreach (GameObject gunObject in gunList)
        {
            if (gunObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public void IncreaseDamage(int value)
    {
        damage += value;
    }

    public int GetAmmo() { return ammo; }

    public void AddAmmo (int value)
    {
        ammo += value;
        UpdateUI();
    }
}
