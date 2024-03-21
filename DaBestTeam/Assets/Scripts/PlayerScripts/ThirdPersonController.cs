using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class ThirdPersonController : MonoBehaviour, IDamage
{
    [Header("--Components--")]
    public CharacterController controller;
    Transform cameraTransform;
    private PlayerInput input;
    Animator animator;
    Vector3 playerVelocity;
    [SerializeField] CanvasGroup deathScreen;
    [SerializeField] GameObject floatingText;

    [Header("--Stats--")]
    [SerializeField] int HP;
    [SerializeField] float Stamina;
    [SerializeField] float staminaDrain;
    [SerializeField] float chargeRate;
    [SerializeField] float playerSpeed;
    [SerializeField] float sprintMod;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [SerializeField] float rotationSpeed;
    private Coroutine recharge;

    [Header("--Restrictions--")]
    public bool canSprint;
    public bool canCrouch;
    public bool canJump;

    [Header("--Current Action--")]
    public bool isSprinting;
    public bool isCrouching;
    public bool isJumping;
    bool isGrounded;
    bool isDead;
    bool staminaDrained;
    bool resetStats;

    //Stat max
    int HPMax;
    float StaminaMax;
    float playerSpeedMax;

    //animation timing
    [SerializeField] float animationSmoothTime = 0.1f;
    [SerializeField] float animationPlayTransition = 0.15f;

    //input actions
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction crouchAction;

    //ints
    int jumpAnimation;
    int moveXAnimationParameterId;
    int moveZAnimationParameterId;

    //vector 2
    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;

    //colors
    Color StaminaColorOrig;
    Color HealthColorOrig;

    private void Awake()
    {
        //components
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
        jumpAnimation = Animator.StringToHash("Jump");
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");

        //input
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        sprintAction = input.actions["Sprint"];
        crouchAction = input.actions["Crouch"];
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        HPMax = HP;
        StaminaMax = Stamina;
        playerSpeedMax = playerSpeed;
        StaminaColorOrig = gameManager.instance.playerStaminaBar.color;
        HealthColorOrig = gameManager.instance.playerHPBar.color;
        updateUI();
        resetStats = PlayerPrefs.GetInt("ResetPlayer", 0) == 1 ? true : false;
        if (resetStats)
        {
            Debug.Log("Reset");
            WeaponSlotManager.instance.ResetWeapons();
            gameManager.instance.ResetCurrency();
            WeaponSlotManager.instance.SaveWeapons();
            gameManager.instance.SaveCurrency();
            PlayerPrefs.SetInt("ResetPlayer", 0);
            PlayerPrefs.Save();
        }
        SaveManager.instance.LoadGame();
    }

    void Update()
    {
        if (!gameManager.instance.isPaused && !isDead)
        {
            Movement();

            if (isSprinting)
            {
                DrainStamina();
                ResetRecharge();
            }
        }
    }

    public void takeDamage(int amount)
    {
        if (!isDead)
        {
            HP -= amount;

            if (floatingText)
                ShowFloatingText(amount);

            StartCoroutine(flashDamage());
            if (HP <= 0)
            {
                isDead = true;

                //death logic
                StartCoroutine(deathEffect());
            }
            updateHealthUI(); 
        }
    }

    private void OnEnable()
    {
        sprintAction.performed += _ => StartSprint();
        sprintAction.canceled += _ => StopSprint();
        crouchAction.performed += _ => StartCrouch();
        crouchAction.canceled += _ => StopCrouch();
    }

    private void OnDisable()
    {
        sprintAction.performed -= _ => StartSprint();
        sprintAction.canceled -= _ => StopSprint();
        crouchAction.performed -= _ => StartCrouch();
        crouchAction.canceled -= _ => StopCrouch();
    }
    private void Movement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        animator.SetFloat(moveXAnimationParameterId, currentAnimationBlendVector.x);
        animator.SetFloat(moveZAnimationParameterId, currentAnimationBlendVector.y);

        // jump logic
        if (jumpAction.triggered && isGrounded && canJump && !isCrouching)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimation, animationPlayTransition);
        }

        //gravity logic 
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //rotate towards camera
        Quaternion TargetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, rotationSpeed * Time.deltaTime);
    }

    private void StartSprint()
    {
        if (canSprint && !isCrouching)
        {
            playerSpeed *= sprintMod;
            isSprinting = true;
        }
    }

    private void StopSprint()
    {
        if (canSprint && !isCrouching)
        {
            playerSpeed = playerSpeedMax;
            isSprinting = false;
        }
    }

    private void DrainStamina()
    {
        Stamina -= staminaDrain * Time.deltaTime;
        if (Stamina < 0)
        {
            Stamina = 0;
            canSprint = false;
            staminaDrained = true;
            isSprinting = false;
            playerSpeed = playerSpeedMax;
        }
        //update Stamina UI logic
        updateStaminaUI();
    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (Stamina < StaminaMax)
        {
            Stamina += chargeRate / 10f;
            if (Stamina > StaminaMax)
            {
                Stamina = StaminaMax;
                staminaDrained = false;
                canSprint = true;
            }
            //update Stamina UI logic 
            updateStaminaUI();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ResetRecharge()
    {
        if (recharge != null) StopCoroutine(recharge);
        recharge = StartCoroutine(RechargeStamina());
    }

    private void StartCrouch()
    {
        if (canCrouch && !isSprinting)
        {
            isCrouching = true;
            canSprint = false;
            if (controller != null)
            {
                controller.height = 0.815f;
                controller.center = new Vector3(0, 0.44f, 0);
            }
            if (animator != null)
                animator.SetLayerWeight(4, 1);

            playerSpeed = playerSpeed / 2;
        }
    }

    private void StopCrouch()
    {
        if (canCrouch && !isSprinting)
        {
            isCrouching = false;
            if (!staminaDrained)
                canSprint = true;
            if (controller != null)
            {
                controller.height = 1.63f;
                controller.center = new Vector3(0, 0.88f, 0);
            }
            if (animator != null)
                animator.SetLayerWeight(4, 0);

            playerSpeed = playerSpeedMax;
        }
    }

    public void HealPlayer(int value)
    {
        HP += value;
        if (HP > HPMax)
        {
            HP = HPMax;
        }
        updateHealthUI();
    }

    public void IncreaseHPMax(int value)
    {
        HPMax += value;
        HealPlayer(value);
        updateHealthUI();
    }

    public void IncreaseSpeed(float value)
    {
        playerSpeedMax += value;
        playerSpeed = playerSpeedMax;
    }

    public void IncreaseStamina(float value)
    {
        StaminaMax += value;
    }

    void updateUI()
    {
        updateHealthUI();
        updateStaminaUI();
    }
    void updateStaminaUI()
    {
        float newAmount = (float)Stamina / StaminaMax;
        gameManager.instance.playerStaminaBar.fillAmount = newAmount;

        if (canSprint)
        {
            if (newAmount > 0.4f)
                gameManager.instance.playerStaminaBar.color = StaminaColorOrig;
            else
                gameManager.instance.playerStaminaBar.color = new Color(1f, 0.5f, 0f);
        }
        else if (!canSprint && staminaDrained)
            gameManager.instance.playerStaminaBar.color = Color.red;
    }

    void updateHealthUI()
    {
        float newAmount = (float)HP / HPMax;
        gameManager.instance.playerHPBar.fillAmount = newAmount;

        if (newAmount >= 0.6f)
            gameManager.instance.playerHPBar.color = HealthColorOrig;
        else if (newAmount < 0.6f && newAmount > 0.3f)
            gameManager.instance.playerHPBar.color = new Color(1f, 0.5f, 0f);
        else
            gameManager.instance.playerHPBar.color = Color.red;
    }

    public void Respawn()
    {
        //HP = HPMax;
        //Stamina = StaminaMax;
        //canSprint = true;
        //deathScreen.alpha = 0f;
        //updateUI();
        //controller.enabled = false;
        //transform.position = gameManager.instance.spawnPoint.transform.position;
        //controller.enabled = true;
    }

    IEnumerator flashDamage()
    {
        if ((float)HP / HPMax > 0.6 )
        {
            UIManager.instance.DamageScreenOne.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            UIManager.instance.DamageScreenOne.SetActive(false); 
        }
        else if ((float)HP / HPMax <= 0.6 && (float)HP / HPMax >= 0.3)
        {
            UIManager.instance.DamageScreenTwo.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            UIManager.instance.DamageScreenTwo.SetActive(false);
        }
        else
        {
            UIManager.instance.DamageScreenThree.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            UIManager.instance.DamageScreenThree.SetActive(false);
        }
    }

    IEnumerator deathEffect()
    {
        float fadeDuration = 3f;
        float elapsedTime = 0f;
        float startAlpha = deathScreen.alpha;
        animator.SetLayerWeight(1, 0);
        animator.SetLayerWeight(2, 0);
        animator.SetLayerWeight(3, 0);
        animator.SetTrigger("Death");
        UIManager.instance.CurrencyDisplay.SetActive(false);
        UIManager.instance.HPDisplay.SetActive(false);
        UIManager.instance.StaminaDisplay.SetActive(false);
        UIManager.instance.AmmoDisplay.SetActive(false);
        WeaponSlotManager.instance.FreezeWeapons(true);
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime/fadeDuration);
            deathScreen.alpha = Mathf.Lerp(startAlpha, 1f, t);
            yield return null;
        }
        gameManager.instance.youLose();
    }

    void ShowFloatingText(int value)
    {
        GameObject text = GameObject.Instantiate(floatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = value.ToString();
    }
}
