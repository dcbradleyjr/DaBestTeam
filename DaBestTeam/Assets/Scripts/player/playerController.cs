using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage, IPushBack, IHealth
{
    [Header("--Components--")]
    [SerializeField] CharacterController controller;
    [SerializeField] Transform headPosition;

    [Header("--Stats--")]
    [Range(1,50)][SerializeField] int HP;
    [Range(0,250)][SerializeField] int Stamina;
    [Range(1,3)][SerializeField] int staminaDrain;
    [Range(1, 20)][SerializeField] float playerSpeed;
    [Range(2, 5)][SerializeField] float sprintMod;
    [Range(0.5f, 2f)][SerializeField] float playerStrafe;
    [Range(1, 3)][SerializeField] float jumpMax;
    [Range(10, 25)][SerializeField] float jumpForce;
    [Range(-50, 0)][SerializeField] float gravity;
    [SerializeField] int pushBackResolve;

    [Header("--Interaction--")]
    [Range(1, 5)][SerializeField] float maxInteractDist;
    [Range(0.1f,5)][SerializeField] float maxInteractRadius;

    public bool canSprint;

    Vector3 pushBack;
    Vector3 move;
    Vector3 playerVelocity;

    int jumpCount;
    int HPOriginal;
    int StaminaOriginal;
    float playerSpeedOriginal;
    public bool isSprinting;
    Color StaminaColorOrig;
    Color HealthColorOrig;
    private Vector3 lastPosition;
    private Vector3 currentVelocity;
    public bool isPlayingSteps;

    
    

    void Start()
    {
        HPOriginal = HP;
        StaminaOriginal = Stamina;
        playerSpeedOriginal = playerSpeed;
        StaminaColorOrig = gameManager.instance.playerStaminaBar.color;
        HealthColorOrig = gameManager.instance.playerHPBar.color;
        Respawn();
        lastPosition = transform.position;
        
    }

    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * maxInteractDist, Color.red);

        sprint();
        if (!gameManager.instance.isPaused)
        {
            movement();
            if (Input.GetButtonDown("Interact"))
                interact(); 
        }
        //velocity
        currentVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    void sprint()
    {
        if (!canSprint)
        playerSpeed = playerSpeedOriginal;

        if (canSprint)
        {
            if (Input.GetButtonDown("Sprint"))
            {
                playerSpeed *= sprintMod;
                isSprinting = true;
                StartCoroutine(StaminaDrainCoroutine());
            }
            else if (Input.GetButtonUp("Sprint"))
            {
                playerSpeed /= sprintMod;
                isSprinting = false;
                StopCoroutine(StaminaDrainCoroutine());
                StartCoroutine(StaminaReplenishCoroutine());
            } 
        }
        else if (isSprinting && Input.GetButtonUp("Sprint")) 
        {
            isSprinting = false;
            StopCoroutine(StaminaDrainCoroutine());
            StartCoroutine(StaminaReplenishCoroutine());
        }
    }

    public void pushBackDir(Vector3 dir)
    {
        pushBack += dir;
    }

    IEnumerator StaminaDrainCoroutine()
    {
        while (isSprinting && Stamina > 0)
        {
            yield return new WaitForSeconds(0.01f);
            Stamina -= staminaDrain;

            if(Stamina == 0)
                canSprint = false;
            updateStaminaUI();
        }
    }

    IEnumerator StaminaReplenishCoroutine()
    {
        while (!isSprinting && Stamina < StaminaOriginal)
        {
            yield return new WaitForSeconds(0.05f);
            Stamina += staminaDrain;
            updateStaminaUI();
        }

        if (!isSprinting && Stamina == StaminaOriginal)
        {
            canSprint = true;
            updateStaminaUI();
        }
    }

    void movement()
    {
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackResolve);

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity = Vector3.zero;
        }

        move = (Input.GetAxis("Horizontal") * transform.right * playerStrafe) + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * playerSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVelocity.y = jumpForce;
            jumpCount++;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);

        if (controller.isGrounded && move.normalized.magnitude > 0.3f && !isPlayingSteps)
        {
            AudioManager.instance.playFootSteps();
        }
    }

    void interact()
    {
        RaycastHit hit;
        //Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxInteractDist))
        {
            Debug.Log(hit.collider.name);
            IInteract interact = hit.collider.GetComponent<IInteract>();
            if(interact != null )
            {
                interact.interact();
            }
        }
    }

    public void takeDamage(int amount)
    {
        AudioManager.instance.hurtSound();
        HP -= amount;
        updateHealthUI();
        StartCoroutine(flashDMG());
        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    public void healHP(int amount)
    {
        if (HP + amount > HPOriginal)
        {
            HP = HPOriginal;
        }
        else
        {
            HP += amount;
        }
        updateHealthUI();
        StartCoroutine(flashHeal());
    }

    void updateUI()
    {
        updateHealthUI();
        updateStaminaUI();
    }
    void updateStaminaUI()
    {
        float newAmount = (float)Stamina / StaminaOriginal;
        gameManager.instance.playerStaminaBar.fillAmount = newAmount;

        if (canSprint)
        {
            if (newAmount > 0.4f)
                gameManager.instance.playerStaminaBar.color = StaminaColorOrig;
            else
                gameManager.instance.playerStaminaBar.color = new Color(1f, 0.5f, 0f); 
        }
        else
         gameManager.instance.playerStaminaBar.color = Color.red;
    }

    void updateHealthUI()
    {
        float newAmount = (float)HP / HPOriginal;
        gameManager.instance.playerHPBar.fillAmount = newAmount;

        if (newAmount >= 0.6f)
            gameManager.instance.playerHPBar.color = HealthColorOrig;
        else if (newAmount < 0.6f && newAmount > 0.3f)
            gameManager.instance.playerHPBar.color = new Color(1f, 0.5f, 0f);
        else
            gameManager.instance.playerHPBar.color = Color.red;
    }


    IEnumerator flashDMG()
    {
        gameManager.instance.FlashDMGPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.FlashDMGPanel.SetActive(false);
    }

    IEnumerator flashHeal()
    {
        gameManager.instance.FlashHealPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.FlashHealPanel.SetActive(false);
    }

    public void Respawn()
    {
        HP = HPOriginal;
        Stamina = StaminaOriginal;
        canSprint = true;
        updateUI();
        controller.enabled = false;
        transform.position = gameManager.instance.SpawnPoint.transform.position;
        controller.enabled = true;
    }

    public Vector3 GetVelocity()
    {
        return currentVelocity;
    }

    
}