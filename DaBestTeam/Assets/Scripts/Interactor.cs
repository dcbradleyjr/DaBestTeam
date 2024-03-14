using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] Transform interactorPoint;
    [SerializeField] float InteractRange;
    [SerializeField] LayerMask interactableMask;
    [SerializeField] InteractionPromptUI interactionPromptUI;
    [SerializeField] int interactableCount;
    [SerializeField] PlayerInput input;
    InputAction interactAction;

    private readonly Collider[] colliders = new Collider[3];

    private IInteract interactable;

    void Start()
    {
        interactAction = input.actions["Interact"];
    }

    void Update()
    {
        interactableCount = Physics.OverlapSphereNonAlloc(interactorPoint.position, InteractRange, colliders, interactableMask);

        if (interactableCount > 0)
        {
            interactable = colliders[0].GetComponent<IInteract>();

            if (interactable != null)
            {
                if (!interactionPromptUI.IsDisplayed) interactionPromptUI.SetUp(interactable.interactPrompt);

                if (interactAction.triggered) interactable.interact();
            }
        }
        else
        {
            if (interactable != null) interactable = null;
            if (interactionPromptUI.IsDisplayed) interactionPromptUI.Close();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactorPoint.position, InteractRange);
    }
}
