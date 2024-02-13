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
        updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (clipSize <= 0 && !isReloading || Input.GetButtonDown("Reload") && !isReloading)
        {
            StartCoroutine(reloadGun());
        }
        else if (Input.GetButtonDown("Shoot") && !isShooting && clipSize > 0)
        {
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPosition.position, transform.rotation);
        clipSize--;
        updateUI();
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator reloadGun()
    {
        isReloading = true;
        StartCoroutine(reloadingVisuals());
        yield return new WaitForSeconds(3.0f);
        clipSize = maxClipSize;
        updateUI();
        isReloading = false;
    }

    void updateUI()
    {
        gameManager.instance.MagazineCount.text = clipSize.ToString();
        gameManager.instance.MagazineMax.text = maxClipSize.ToString();
    }

    IEnumerator reloadingVisuals()
    {
        gameManager.instance.ReloadIndicator.text = "Reloading";
        yield return new WaitForSeconds(0.7f);
        gameManager.instance.ReloadIndicator.text = "Reloading.";
        yield return new WaitForSeconds(0.7f);
        gameManager.instance.ReloadIndicator.text = "Reloading..";
        yield return new WaitForSeconds(0.7f);
        gameManager.instance.ReloadIndicator.text = "Reloading...";
        yield return new WaitForSeconds(0.7f);
        gameManager.instance.ReloadIndicator.text = "";
    }
}
