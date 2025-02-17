using UnityEngine;
using UnityEngine.AI;

public class Attack : Node
{
    private readonly NavMeshAgent agent;
    private readonly LifeManager playerLifeManager;
    private readonly float attackInterval;
    private readonly float damages;
    private readonly float attackRange;
    private float lastAttackTime;

    public Attack(LifeManager playerLifeManager, float attackInterval, float damages, NavMeshAgent agent, float attackRange)
    {
        this.playerLifeManager = playerLifeManager;
        this.attackInterval = attackInterval;
        this.damages = damages;
        lastAttackTime = -attackInterval;
        this.attackRange = attackRange;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(playerLifeManager.transform.position, agent.transform.position);
        if (distance > attackRange)
        {
            return NodeState.FAILURE;
        }

        if (Time.time < lastAttackTime + attackInterval)
        {
            return NodeState.SUCCESS;
        }

        playerLifeManager.TakeDamage(damages);
        lastAttackTime = Time.time;

        return NodeState.SUCCESS;
    }

}