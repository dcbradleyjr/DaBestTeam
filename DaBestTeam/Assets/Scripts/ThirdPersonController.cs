using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class ThirdPersonController : MonoBehaviour, IDamage
{
    public CharacterController controller;
    private PlayerInput input;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    [Header("--Stats--")]
    [SerializeField] int HP;
    [SerializeField] float Stamina;
    [SerializeField] float staminaDrain;
    [SerializeField] float chargeRate;
    private Coroutine recharge;
    [SerializeField] float playerSpeed;
    [SerializeField] float sprintMod;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [SerializeField] float rotationSpeed;
    public bool canSprint;

    int HPMax;
    float StaminaMax;
    float playerSpeedMax;
    public bool isSprinting;

    [SerializeField] float animationSmoothTime = 0.1f;
    [SerializeField] float animationPlayTransition = 0.15f;



    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    Animator animator;
    int jumpAnimation;
    int moveXAnimationParameterId;
    int moveZAnimationParameterId;

    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;

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
    }

    void Update()
    {
        Movement();

        if (isSprinting)
        {
            DrainStamina();
            ResetRecharge();
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            //death logic
        }
        updateHealthUI();
    }

    private void OnEnable()
    {
        sprintAction.performed += _ => StartSprint();
        sprintAction.canceled += _ => StopSprint();
    }

    private void OnDisable()
    {
        sprintAction.performed -= _ => StartSprint();
        sprintAction.canceled -= _ => StopSprint();
    }
    private void Movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
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

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimation, animationPlayTransition);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //rotate towards camera

        Quaternion TargetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, rotationSpeed * Time.deltaTime);
    }

    private void StartSprint()
    {
        if (canSprint)
        {
            playerSpeed *= sprintMod;
            isSprinting = true; 
        }
    }

    private void StopSprint()
    {
        if (canSprint)
        {
            playerSpeed /= sprintMod;
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
        else
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
}
