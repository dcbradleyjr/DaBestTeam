using UnityEngine;
using UnityEngine.InputSystem;


public class MeleeSlot : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] Animator animator;
    [SerializeField] float animationPlayTransition = 0.15f;

    InputAction meleeAction;
    int meleeAnimation;
    bool canMelee;

    void Awake()
    {
        meleeAction = input.actions["Melee"];
        meleeAnimation = Animator.StringToHash("Melee");
    }

    private void OnEnable()
    {
        meleeAction.performed += _ => MeleeAttack();
        canMelee = true;
        Debug.Log("LayerWeight on");
        animator.SetLayerWeight(2, 1);
    }

    private void OnDisable()
    {
        meleeAction.performed -= _ => MeleeAttack();
        canMelee = false;
        Debug.Log("LayerWeight off");
        animator.SetLayerWeight(2, 0);
    }

    private void MeleeAttack()
    {
        if (canMelee)
        {
            animator.CrossFade(meleeAnimation, animationPlayTransition);
        }
    }
}
