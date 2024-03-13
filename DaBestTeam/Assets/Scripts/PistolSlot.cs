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

    // Start is called before the first frame update
    void Awake()
    {
       shootAction = input.actions["Shoot"];
       cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
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
            Instantiate(muzzleFlash,shootPoint.position, shootPoint.rotation);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
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

    //public void DisableShooting()
    //{
    //    OnDisable(); // Manually call OnDisable
    //    gameObject.SetActive(false);
    //}
}


