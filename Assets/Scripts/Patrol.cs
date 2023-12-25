using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    int checkpointIndex = 0;

    public Patrol(Transform npc, NavMeshAgent agent, Animator anim, Transform player) 
                    : base(npc, agent, anim, player) { }

    const float speed = 1f;
    const bool isStopped = false;
    const string patrolState = "isWalking";

    protected override void Enter()
    {
        checkpointIndex = GetClosestCheckpointIndex();
        agent.SetDestination(Checkpoints[checkpointIndex].position);
        
        SetNPCProps(speed, isStopped, patrolState);
        base.Enter();
    }

    protected override void Update()
    {
        if (Checkpoints[checkpointIndex] != null) 
            agent.SetDestination(Checkpoints[checkpointIndex].position);
        
        if (IsSpottedPlayer())
        {
            nextState = new Chase(npc, agent, anim, player);
            base.Exit();
        }
        else if (agent.hasPath && agent.remainingDistance < 1f)
            checkpointIndex = checkpointIndex > Checkpoints.Count - 1 ? 0 : checkpointIndex++;
    }

    protected override void Exit() => anim.ResetTrigger(patrolState);

}
