/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu()]

public class EnemyAIConfig : ScriptableObject
{
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float dieForce = 10.0f;
    public float maxSightDistance = 5.0f;
}
public enum AIStateId
{
    ChasePlayer,
    Death,
    Idle
}
public interface EnemyAIState
{
    AIStateId GetId();
    void Enter(EnemyAIAgent agent);
    void Update(EnemyAIAgent agent);
    void Exit(EnemyAIAgent agent);
}
public class EnemyAIStateMachine
{
    public EnemyAIState[] states;
    public EnemyAIAgent agent;
    public AIStateId currentState;

    public EnemyAIStateMachine(EnemyAIAgent agent)
    {
        this.agent = agent;
        int numberstates = System.Enum.GetNames(typeof(AIStateId)).Length;
        states = new EnemyAIState[numberstates];
        
    }

    public void RegisterState(EnemyAIState state)
    {
        int i = (int)state.GetId();
        states[i] = state;
    }

    public EnemyAIState GetState(AIStateId stateId)
    {
        int i = (int)stateId;
        return states[i];
    }

    public void Update()
    {
        GetState(currentState)?.Update(agent); 
    }

    public void ChangeState(AIStateId newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}
public class EnemyAIChasePlayerState : EnemyAIState
{


    float timer = 0.0f;

    public AIStateId GetId()
    {
        return AIStateId.ChasePlayer;
    }

    public void Enter(EnemyAIAgent agent)
    {

    }

    public void Exit(EnemyAIAgent agent)
    {

    }

    public void Update(EnemyAIAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }
        timer -= Time.deltaTime;

        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        if (timer < 0.0f)
        {
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            timer = agent.config.maxTime;
        }

    }
}
public class EnemyAIDeathState : EnemyAIState
{
    public Vector3 direction;

    public void Enter(EnemyAIAgent agent)
    {
        //agent.ragdoll.ActivateRagDoll();
        direction.y = 1;
        //agent.ragdoll.Applyforce(direction * agent.config.dieForce);
        //agent.ui.healthbar.gameObject.SetActive(false);
        agent.mesh.updateWhenOffscreen = true;
    }

    public void Exit(EnemyAIAgent agent)
    {

    }

    public AIStateId GetId()
    {
        return AIStateId.Death;
    }

    public void Update(EnemyAIAgent agent)
    {

    }
}
public class EnemyAIIdleState : EnemyAIState
{
    
    public void Enter(EnemyAIAgent agent)
    {

    }

    public void Exit(EnemyAIAgent agent)
    {

    }

    public AIStateId GetId()
    {
        return AIStateId.Idle;
    }

    public void Update(EnemyAIAgent agent)
    {
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        if (playerDirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }

        Vector3 agentDirection = agent.transform.forward;

        playerDirection.Normalize();

        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        if (dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AIStateId.ChasePlayer);
        }

    }
}*/