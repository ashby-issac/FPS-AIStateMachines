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
        agent.SetDestination(checkpoints[checkpointIndex].position);
        
        SetNPCProperties(speed, isStopped, patrolState);
        base.Enter();
    }

    protected override void Update()
    {
        if (IsSpottedPlayer())
        {
            nextState = new Chase(npc, agent, anim, player);
            base.Exit();
        }
        else if (agent.hasPath && agent.remainingDistance < 1f)
        {
            checkpointIndex = checkpointIndex > checkpoints.Count - 1 ? 0 : ++checkpointIndex;
            Debug.Log($":: Checkpoint: {checkpointIndex} || (Count - 1): {checkpoints.Count - 1}");
            if (checkpointIndex <= checkpoints.Count - 1)
                agent.SetDestination(checkpoints[checkpointIndex].position);
        }
    }

    protected override void Exit() => anim.ResetTrigger(patrolState);

}
