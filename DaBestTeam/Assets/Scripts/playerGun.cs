using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGun : MonoBehaviour
{
    [Header("--Components--")]
    [SerializeField] Transform shootPosition;
    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject bullet;

    [Header("--Attributes--")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] float shootRate;
    [SerializeField] float reloadRate;
    [SerializeField] int clipSize;

    bool isShooting;
    bool isReloading;
    bool isAuto;
    int maxClipSize;
    int selectedGun;

    // Start is called before the first frame update
    void Start()
    {
        maxClipSize = clipSize;
        updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            if (gunList.Count > 0)
            {
                if(!isReloading)
                selectGun();

                if (CanReload())
                {
                    StartCoroutine(reloadGun());
                }
                else if (CanShootAuto() || CanShootSemi())
                {
                    StartCoroutine(shoot());
                }  
            }
        }
    }

    IEnumerator shoot()
    {
        AudioManager.instance.shootSound();
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
        yield return new WaitForSeconds(reloadRate);
        clipSize = maxClipSize;
        updateUI();
        isReloading = false;
    }

    void updateUI()
    {
        if (maxClipSize == 0)
        {
            gameManager.instance.MagazineCount.gameObject.SetActive(false);
            gameManager.instance.MagazineMax.gameObject.SetActive(false);
            gameManager.instance.AmmoDivider.gameObject.SetActive(false);
        }
        else
        {
            gameManager.instance.MagazineCount.gameObject.SetActive(true);
            gameManager.instance.MagazineMax.gameObject.SetActive(true);
            gameManager.instance.AmmoDivider.gameObject.SetActive(true);
        }

        gameManager.instance.MagazineCount.text = clipSize.ToString();
        gameManager.instance.MagazineMax.text = maxClipSize.ToString();
    }

    IEnumerator reloadingVisuals()
    {
        gameManager.instance.ReloadIndicator.text = "Reloading";
        yield return new WaitForSeconds(reloadRate / 4);
        gameManager.instance.ReloadIndicator.text = "Reloading.";
        yield return new WaitForSeconds(reloadRate / 4);
        gameManager.instance.ReloadIndicator.text = "Reloading..";
        yield return new WaitForSeconds(reloadRate / 4);
        gameManager.instance.ReloadIndicator.text = "Reloading...";
        yield return new WaitForSeconds(reloadRate / 4);
        gameManager.instance.ReloadIndicator.text = "";
    }

    public void getGunStats(gunStats gun)
    {
        if (gunList.Count != 0)
            saveAmmo();

        gunList.Add(gun);

        shootRate = gun.shootRate;
        reloadRate = gun.reloadRate;
        maxClipSize = gun.ammoMax;
        clipSize = gun.ammoCur;
        bullet = gun.bullet;
        isAuto = gun.isAuto;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = gunList.Count - 1;
        updateUI();
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            saveAmmo();
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            saveAmmo();
            selectedGun--;
            changeGun();
        }
    }

    void changeGun()
    {
        shootRate = gunList[selectedGun].shootRate;
        reloadRate = gunList[selectedGun].reloadRate;
        maxClipSize = gunList[selectedGun].ammoMax;
        clipSize = gunList[selectedGun].ammoCur;
        bullet = gunList[selectedGun].bullet;
        isAuto = gunList[selectedGun].isAuto;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;

        updateUI();
    }

    void saveAmmo()
    {
        gunList[selectedGun].ammoCur = clipSize;
    }
    bool CanShootSemi()
    {
        return Input.GetButtonDown("Shoot") && !isShooting && clipSize > 0 && !isReloading && !isAuto;
    }
    bool CanShootAuto()
    {
        return Input.GetButton("Shoot") && !isShooting && clipSize > 0 && !isReloading && isAuto;
    }

    bool CanReload()
    {
        return clipSize <= 0 && !isReloading || Input.GetButtonDown("Reload") && !isReloading && clipSize != maxClipSize;
    }
}
