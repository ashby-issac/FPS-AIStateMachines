using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

internal class Chase : State
{
    public Chase(Transform npc, NavMeshAgent agent, Animator anim, Transform player)
                    : base(npc, agent, anim, player) { }

    const float speed = 3.5f;
    const bool isStopped = false;
    const string runState = "isRunning";

    protected override void Enter()
    {
        SetNPCProps(speed, isStopped, runState);
        base.Enter();
    }

    protected override void Update()
    {
        agent.SetDestination(player.position);
        if (!IsSpottedPlayer())
        {
            nextState = new Idle(npc, agent, anim, player);
            base.Exit();
        }
        else if (IsInShootRange())
        {
            nextState = new Attack(npc, agent, anim, player);
            base.Exit();
        }
    }

    protected override void Exit() => anim.ResetTrigger(runState);
}