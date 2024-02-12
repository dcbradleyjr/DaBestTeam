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
    int maxClipSize;

    // Start is called before the first frame update
    void Start()
    {
        maxClipSize = clipSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (clipSize <= 0 || Input.GetButton("Reload"))
        {
            StartCoroutine(reloadGun());
        }
        else if (Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPosition.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        clipSize--;
        isShooting = false;
    }

    IEnumerator reloadGun()
    {
        yield return new WaitForSeconds(3.0f);
        clipSize = maxClipSize;
    }
}
