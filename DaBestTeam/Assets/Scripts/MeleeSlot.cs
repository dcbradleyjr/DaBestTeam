using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class MeleeSlot : MonoBehaviour
{
    [SerializeField] int dmgAmount;
    [SerializeField] int knockBack;
    [SerializeField] PlayerInput input;
    [SerializeField] Animator animator;
    [SerializeField] GameObject bloodSplat;
    [SerializeField] float animationPlayTransition = 0.15f;

    InputAction meleeAction;
    int meleeAnimation;
    bool canMelee;
    public bool isAttacking;

    void Awake()
    {
        meleeAction = input.actions["Melee"];
        meleeAnimation = Animator.StringToHash("Melee");
    }

    private void OnEnable()
    {
        meleeAction.performed += _ => MeleeAttack();
        canMelee = true;
        isAttacking = false;
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
        if (canMelee && !isAttacking)
        {
            animator.CrossFade(meleeAnimation, animationPlayTransition);
            StartCoroutine(MeleeDamage());
        }
    }

    IEnumerator MeleeDamage()
    {
        isAttacking = true;
        if (canMelee)
        {
            yield return new WaitForSeconds(0.5f);
            CheckForDamageable();
        }
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    public void CheckForDamageable()
    {

        Vector3 boxSize = new Vector3(1.5f, 3f, 3.5f);
        Vector3 boxCenter = transform.position + transform.forward * 1.2f;

        Collider[] colliders = Physics.OverlapBox(boxCenter, boxSize / 2f);
        foreach (Collider collider in colliders)
        {
            IDamage dmg = collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(dmgAmount);
                Instantiate(bloodSplat, collider.transform.position + new Vector3(0,1,0), Quaternion.identity);
            }

            IPushBack pushBack = collider.GetComponent<IPushBack>();

            if (pushBack != null)
            {
                pushBack.pushBackDir((collider.transform.position + new Vector3(0, 1, 0) + transform.position).normalized * knockBack);
            }
        }
    }
}
