using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField] Transform player;

    Animator anim;
    NavMeshAgent agent;
    State currentState;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        currentState = new Idle(transform, agent, anim, player);
    }

    void Update()
    {
        if (currentState != null)
            currentState = currentState.ProcessStates();
    }
}
