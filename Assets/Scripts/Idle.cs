using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    public Idle(Transform npc, NavMeshAgent agent, Animator anim, Transform player)
                : base(npc, agent, anim, player) { }

    const float speed = 0f;
    const bool isStopped = true;
    const string idleState = "isIdle";

    protected override void Enter()
    {
        SetNPCProps(speed, isStopped, idleState);
        base.Enter();
    }

    protected override void Update()
    {
        if (IsSpottedPlayer())
        {
            nextState = new Chase(npc, agent, anim, player);
            base.Exit();
        }
        else if (Random.Range(0, 5000) < 10)
        {
            nextState = new Patrol(npc, agent, anim, player);
            base.Exit();
        }
    }

    protected override void Exit() => anim.ResetTrigger(idleState);
}
