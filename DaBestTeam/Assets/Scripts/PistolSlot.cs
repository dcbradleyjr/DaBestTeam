using UnityEngine;
using UnityEngine.InputSystem;

public class PistolSlot : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Transform shootPoint;
    [SerializeField] Animator animator;
    [SerializeField] float bulletHitMissDistance = 25f;
    Transform cameraTransform;
    InputAction shootAction;
    bool canShoot;

    void Awake()
    {
       shootAction = input.actions["Shoot"];
       cameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
        canShoot = true;
        animator.SetLayerWeight(1, 1);
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();
        canShoot = false;
        animator.SetLayerWeight(1, 0);
    }

    private void ShootGun()
    {
        if (canShoot)
        {
            RaycastHit hit;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            GameObject flash = Instantiate(muzzleFlash,shootPoint.position, shootPoint.rotation);
            Destroy(flash, 0.5f);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
            }
            else
            {
                bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
                bulletController.hit = false;
            } 
        }
    }
}


