using System.Collections;
using System.Collections.Generic;
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

    Vector3 move;
    Vector3 playerVelocity;
    int jumpCount;
    int HPOriginal;

    void Start()
    {
        HPOriginal = HP;
        updateUI();
    }

    void Update()
    {
        movement();
        
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
}