using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    public Attack(Transform npc, NavMeshAgent agent, Animator anim, Transform player)
                    : base(npc, agent, anim, player) { }

    const float speed = 0f;
    const bool isStopped = true;
    const string animState = "isShooting";

    protected override void Enter()
    {
        SetNPCProps(speed, isStopped, animState);
        base.Enter();
    }

    protected override void Update()
    {
        var directionToPlayer = GetVectorToPlayer(isNormalized: true);
        var lookRotation = Quaternion.LookRotation(directionToPlayer);
        npc.rotation = Quaternion.Slerp(npc.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        if (!IsInShootRange())
        {
            nextState = new Chase(npc, agent, anim, player);
            base.Exit();
        }
    }

    protected override void Exit()
    {
        anim.ResetTrigger(animState);
        base.Exit();
    }
}
