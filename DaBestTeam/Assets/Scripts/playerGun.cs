using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGun : MonoBehaviour
{

    [SerializeField] Transform shootPosition;

    [SerializeField] GameObject bullet;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int clipSize;

    bool isShooting;
    bool isReloading;
    int maxClipSize;

    // Start is called before the first frame update
    void Start()
    {
        maxClipSize = clipSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (clipSize <= 0 && !isReloading || Input.GetButton("Reload") && !isReloading)
        {
            StartCoroutine(reloadGun());
        }
        else if (Input.GetButton("Shoot") && !isShooting && clipSize > 0)
        {
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPosition.position, transform.rotation);
        clipSize--;
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator reloadGun()
    {
        isReloading = true;
        yield return new WaitForSeconds(3.0f);
        clipSize = maxClipSize;
        isReloading = false;
    }
}
