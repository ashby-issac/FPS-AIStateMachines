using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STAGE
    {
        Enter, Update, Exit
    }

    protected STAGE stage;

    protected State nextState;

    protected Transform npc, player;
    protected NavMeshAgent agent;
    protected Animator anim;

    protected float visionDistance = 15f;
    protected float visionAngle = 60f;
    protected float closeRangeDistance = 3f;
    protected float shootRange = 8f;
    protected float rotationSpeed = 5f;

    protected List<Transform> checkpoints => GameEnvironment.Instance.Checkpoints;

    protected virtual void Enter() => stage = STAGE.Update;
    protected virtual void Update() => stage = STAGE.Update;
    protected virtual void Exit() => stage = STAGE.Exit;

    public State(Transform npc, NavMeshAgent agent, Animator anim, Transform player)
    {
        stage = STAGE.Enter;

        this.npc = npc;
        this.agent = agent;
        this.anim = anim;
        this.player = player;
    }

    public State ProcessStates()
    {
        if (stage == STAGE.Enter) Enter();
        if (stage == STAGE.Update) Update();
        if (stage == STAGE.Exit)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    protected Vector3 GetVectorToPlayer(bool isNormalized = false) => isNormalized ? (player.position - npc.position).normalized
                                                                                            : (player.position - npc.position);

    protected bool IsSpottedPlayer()
    {
        var direction = GetVectorToPlayer(isNormalized: true);
        var npcAngle = Vector3.Angle(direction, npc.forward);
        return GetVectorToPlayer().magnitude < visionDistance && npcAngle < visionAngle;
    }

    protected bool IsInShootRange() => GetVectorToPlayer().magnitude < shootRange;

    protected int GetClosestCheckpointIndex()
    {
        int checkpointIndex = 0;
        var checkpoints = GameEnvironment.Instance.Checkpoints;
        var closestCheckpointDistance = Mathf.Infinity;

        for (int i = 0; i < checkpoints.Count; i++)
        {
            var distanceToCheckpoint = Vector3.Distance(npc.position, checkpoints[i].position);
            if (distanceToCheckpoint < closestCheckpointDistance)
            {
                closestCheckpointDistance = distanceToCheckpoint;
                checkpointIndex = i;
            }
        }

        return checkpointIndex;
    }

    protected void SetNPCProperties(float moveSpeed, bool isMoving, string animState)
    {
        agent.speed = moveSpeed;
        agent.isStopped = isMoving;
        anim.SetTrigger(animState);
    }
}
