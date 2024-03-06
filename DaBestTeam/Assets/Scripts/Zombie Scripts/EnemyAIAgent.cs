using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIAgent : MonoBehaviour
{
    public EnemyAIStateMachine stateMachine;
    public AIStateId startingState; 
    public NavMeshAgent navMeshAgent;
    public EnemyAIConfig config;
    public SkinnedMeshRenderer mesh;
    public Transform playerTransform;


    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new EnemyAIStateMachine(this);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;   
        stateMachine.RegisterState(new EnemyAIChasePlayerState());
        stateMachine.RegisterState(new EnemyAIDeathState());
        stateMachine.RegisterState(new EnemyAIIdleState());
        stateMachine.ChangeState(startingState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
