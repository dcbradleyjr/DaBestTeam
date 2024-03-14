using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class ThirdPersonController : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput input;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    [Header("--Stats--")]
    [SerializeField] int HP;
    [SerializeField] int Stamina;
    [SerializeField] int staminaDrain;
    [SerializeField] float playerSpeed = 2.0f;
    [SerializeField] float sprintMod;
    [SerializeField] float jumpHeight = 1.0f;
    [SerializeField] float gravityValue = -9.81f;
    [SerializeField] float rotationSpeed = 4f;
    public bool canSprint;


    int jumpCount;
    int HPOriginal;
    int StaminaOriginal;
    float playerSpeedOriginal;
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

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        if (cameraTransform == null)
            Debug.Log("Not found");
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        sprintAction = input.actions["Sprint"];

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        animator = GetComponent<Animator>();
        jumpAnimation = Animator.StringToHash("Jump");
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");
    }

    void Update()
    {
        Movement();
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
            playerSpeed *= sprintMod;
            isSprinting = true;        
    }

    private void StopSprint()
    {
        playerSpeed /= sprintMod;
        isSprinting = false;
    }
}
