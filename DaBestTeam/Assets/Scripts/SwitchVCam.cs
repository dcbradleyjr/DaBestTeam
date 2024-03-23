 using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{

    [SerializeField] PlayerInput input;
    [SerializeField] int regularPriority;
    [SerializeField] int aimPriority;
    [SerializeField] public Canvas thirdPersonCanvas;
    [SerializeField] public Canvas aimCanvas;

    [SerializeField] Image RegularReticle;
    [SerializeField] Image AimReticle;


    [SerializeField] Sprite meleeReticle;
    [SerializeField] Sprite pistolRegularReticle;
    [SerializeField] Sprite pistolAimReticle;
    [SerializeField] Sprite gunRegularReticle;
    [SerializeField] Sprite gunAimReticle;

    public CinemachineVirtualCamera AimCam;
    public CinemachineVirtualCamera ThirdPersonCam;
    InputAction aimAction;

    public bool isAiming;
    bool isPaused;


    private void Awake()
    {
        AimCam = GetComponent<CinemachineVirtualCamera>();
        ThirdPersonCam = GameObject.FindWithTag("ThirdPersonCinemachine").GetComponent<CinemachineVirtualCamera>();
        aimAction = input.actions["Aim"];
    }
    private void Update()
    {
        if (gameManager.instance.isPaused)
        {
            thirdPersonCanvas.enabled = false;
            aimCanvas.enabled = false;
            isPaused = true;
            AimCam.enabled = false;
            ThirdPersonCam.enabled = false;
        }
        else
        {
            if (isPaused)
            {
                CancelAim();
                isPaused = false;
                AimCam.enabled = true;
                ThirdPersonCam.enabled = true;
            }
        }

        if (RegularReticle.sprite == null)
        {
            RegularReticle.sprite = meleeReticle;
        }

        if (AimReticle.sprite == null)
        {
            AimReticle.sprite = meleeReticle;
        }
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }
    private void StartAim()
    {
        if (WeaponSlotManager.instance.isPistolActive || WeaponSlotManager.instance.isGunActive)
        {
            isAiming = true;
            AimCam.Priority = aimPriority;
            if (aimCanvas != null)
                aimCanvas.enabled = true;
            if (thirdPersonCanvas != null)
                thirdPersonCanvas.enabled = false; 
        }
    }

    private void CancelAim()
    {
        isAiming = false;
        AimCam.Priority = regularPriority;
        if (aimCanvas != null)
            aimCanvas.enabled = false;
        if (thirdPersonCanvas != null)
            thirdPersonCanvas.enabled = true;
    }

    public void MeleeReticle()
    {
        RegularReticle.sprite = meleeReticle;
        AimReticle.sprite = meleeReticle;
    }

    public void PistolReticle()
    {
        RegularReticle.sprite = pistolRegularReticle;
        AimReticle.sprite = pistolAimReticle;
    }

    public void GunReticle()
    {
        RegularReticle.sprite = gunRegularReticle;
        AimReticle.sprite = gunAimReticle;
    }
}
