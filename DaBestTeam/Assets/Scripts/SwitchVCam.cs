using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{

    [SerializeField] PlayerInput input;
    [SerializeField] int priorityBoost;
    [SerializeField] Canvas thirdPersonCanvas;
    [SerializeField] Canvas aimCanvas;

    CinemachineVirtualCamera cam;
    InputAction aimAction;


    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        aimAction = input.actions["Aim"];
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
        cam.Priority += priorityBoost;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
    }

    private void CancelAim()
    {
        cam.Priority -= priorityBoost;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
}
