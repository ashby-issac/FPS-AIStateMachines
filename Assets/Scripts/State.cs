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

    protected float visionDistance = 20f, visionAngle = 60f, closeRangeDistance = 3f, shootRange = 8f, rotationSpeed = 5f;

    protected List<Transform> Checkpoints => GameEnvironment.Instance.Checkpoints;

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
        int checkpointIndex = 0, checkpointCounter = 0;
        var checkpoints = GameEnvironment.Instance.Checkpoints;
        var closestCheckpointDistance = Mathf.Infinity;

        checkpoints.ForEach(checkpoint =>
        {
            var distanceToCheckpoint = Vector3.Distance(npc.position, checkpoint.position);
            if (distanceToCheckpoint < closestCheckpointDistance)
            {
                closestCheckpointDistance = distanceToCheckpoint;
                checkpointIndex = checkpointCounter;
            }
            checkpointCounter++;
        });

        return checkpointIndex;
    }

    protected void SetNPCProps(float moveSpeed, bool isMoving, string animState)
    {
        agent.speed = moveSpeed;
        agent.isStopped = isMoving;
        anim.SetTrigger(animState);
    }
}
