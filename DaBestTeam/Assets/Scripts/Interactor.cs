using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] Transform interactorPoint;
    [SerializeField] float InteractRange;
    [SerializeField] LayerMask interactableMask;
    [SerializeField] int interactableCount;
    [SerializeField] PlayerInput input;
    InputAction interactAction;

    private readonly Collider[] colliders = new Collider[3];

    void Start()
    {
        interactAction = input.actions["Interact"];
    }

    void Update()
    {
        interactableCount = Physics.OverlapSphereNonAlloc(interactorPoint.position, InteractRange, colliders, interactableMask);

        if (interactableCount > 0 )
        {
            var interactable = colliders[0].GetComponent<IInteract>();

            if ( interactable != null && interactAction.triggered )
            {
                interactable.interact();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactorPoint.position, InteractRange);
    }
}
