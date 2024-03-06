using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;

    [HideInInspector]
    public float currentHealth;
    EnemyAIAgent agent;
    SkinnedMeshRenderer skinnedMeshRenderer;
    

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    void Start()
    {
        agent = GetComponent<EnemyAIAgent>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        currentHealth = maxHealth;

        var rigidBodies = GetComponentInChildren<Rigidbody>();
    }


    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        if (currentHealth <= 0.0f)
        {
            Die(direction);
        }
    }

    private void Die(Vector3 direction)
    {
        EnemyAIDeathState deathState = agent.stateMachine.GetState(AIStateId.Death) as EnemyAIDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AIStateId.Death);
    }
    
    void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.red * intensity;
    }
}
