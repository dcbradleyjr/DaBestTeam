using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] Transform headPosition;


    [SerializeField] int HP;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpMax;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    [SerializeField] float maxInteractDist;

    Vector3 move;
    Vector3 playerVelocity;
    int jumpCount;
    int HPOriginal;

    void Start()
    {
        HPOriginal = HP;
        Respawn();
    }

    void Update()
    {
        movement();

        if (Input.GetButtonDown("Interact"))
        interact();
    }
    void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity = Vector3.zero;
        }

        move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * playerSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVelocity.y = jumpForce;
            jumpCount++;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, maxInteractDist))
        {
            Debug.Log(hit.collider.name);
            IInteract interact = hit.collider.GetComponent<IInteract>();
            if(interact != null )
            {
                interact.collect();
            }
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        updateUI();
        StartCoroutine(flashDMG());
        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    void updateUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOriginal;
    }

    IEnumerator flashDMG()
    {
        gameManager.instance.FlashDMGPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.FlashDMGPanel.SetActive(false);
    }

    public void Respawn()
    {
        HP = HPOriginal;
        updateUI();
        controller.enabled = false;
        transform.position = gameManager.instance.SpawnPoint.transform.position;
        controller.enabled = true;
    }
}