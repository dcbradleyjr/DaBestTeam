using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSlot : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Transform shootPoint;
    [SerializeField] Animator animator;


    [SerializeField] int damage;
    [SerializeField] int clipSize;
    [SerializeField] int maxClipsize;
    [SerializeField] float reloadRate;
    [SerializeField] float fireRate;
    [SerializeField] float bulletDistance = 25f;
    Transform cameraTransform;
    InputAction shootAction;
    bool canShoot;
    public bool toggleShoot;
    public bool isShooting;
    public bool isAuto;

    void Awake()
    {
        shootAction = input.actions["Shoot"];
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (toggleShoot & isAuto &!isShooting)
            StartCoroutine(ShootGun());
    }

    private void OnEnable()
    {
        shootAction.performed += _ => StartShooting();
        shootAction.canceled += _ => StopShooting();
        canShoot = true;
        animator.SetLayerWeight(3, 1);
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => StartShooting();
        shootAction.canceled -= _ => StopShooting();
        canShoot = false;
        isShooting = false;
        StopAllCoroutines();
        animator.SetLayerWeight(3, 0);
    }

    private void StartShooting()
    {
        toggleShoot = true;
        if(!isShooting && canShoot)
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
            Instantiate(muzzleFlash, shootPoint.position, shootPoint.rotation);
            BulletController bulletController = bullet.GetComponent<BulletController>();
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
}
