using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MeleeSlot : MonoBehaviour
{
    [Header("---Components---")]
    [SerializeField] PlayerInput input;
    [SerializeField] Animator animator;
    [SerializeField] GameObject bloodSplat;
    [SerializeField] float animationPlayTransition = 0.15f;
    [SerializeField] Transform HitPoint;

    [Header("---MeleeList---")]
    [SerializeField] List<GameObject> meleeList;
    [SerializeField] List<Melee> meleeStats;
    [SerializeField] int currentGunIndex;

    [Header("---Stats---")]
    [SerializeField] string meleeName;
    [SerializeField] int dmgAmount;
    [SerializeField] int knockBack;
    [SerializeField] float meleeRange;

    InputAction meleeAction;
    int meleeAnimation;
    bool canMelee;
    public bool isAttacking;

    void Awake()
    {
        meleeAction = input.actions["Melee"];
        meleeAnimation = Animator.StringToHash("Melee");
    }

    private void Update()
    {
        if (!IsAnyMeleeActive() && currentGunIndex != -1)
        {
            ToggleMelee(currentGunIndex);
        }
    }

    private void OnEnable()
    {
        meleeAction.performed += _ => MeleeAttack();
        canMelee = true;
        isAttacking = false;
        animator.SetLayerWeight(2, 1);
    }

    private void OnDisable()
    {
        meleeAction.performed -= _ => MeleeAttack();
        canMelee = false;
        animator.SetLayerWeight(2, 0);
    }

    public void ToggleMelee(int index)
    {
        if (index >= 0 && index < meleeList.Count)
        {
            DisableAllMelee();

            meleeList[index].SetActive(true);
            currentGunIndex = index;
            TransferStats(meleeStats[index]);
        }
        else
        {
            Debug.LogError("Invalid gun index!");
        }
    }

    private void TransferStats(Melee melee)
    {
        meleeName = melee.meleeName;
        dmgAmount = melee.dmgAmount;
        knockBack = melee.knockBack;
        meleeRange = melee.meleeRange;
    }

    public void DisableAllMelee()
    {
        foreach (GameObject gunObject in meleeList)
        {
            gunObject.SetActive(false);
        }
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
        Collider[] colliders = Physics.OverlapSphere(HitPoint.position, meleeRange);
        foreach (Collider collider in colliders)
        {
            IDamage dmg = collider.GetComponent<IDamage>();

            if (dmg != null && !collider.CompareTag("Player") && !collider.isTrigger)
            {
                dmg.takeDamage(dmgAmount);
                GameObject blood = Instantiate(bloodSplat, collider.transform.position + new Vector3(0,1,0), Quaternion.identity);
                Destroy(blood, 0.5f);
            }

            IPushBack pushBack = collider.GetComponent<IPushBack>();

            if (pushBack != null)
            {
                pushBack.pushBackDir((collider.transform.position + new Vector3(0, 1, 0) + transform.position).normalized * knockBack);
            }
        }
    }

    public bool IsAnyMeleeActive()
    {
        foreach (GameObject gunObject in meleeList)
        {
            if (gunObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(HitPoint.position, meleeRange);
    }

    public void IncreaseDamage(int value)
    {
        dmgAmount += value;
    }

    public string GetName() { return meleeName; }

}
