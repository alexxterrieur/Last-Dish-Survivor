using UnityEngine;
using UnityEngine.AI;

public class CanAttack : Node
{
    public NavMeshAgent agent;
    public GameObject player;
    public float attackRange;

    public CanAttack(NavMeshAgent agent, GameObject player, float attackRange)
    {
        this.agent = agent;
        this.player = player;
        this.attackRange = attackRange;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(agent.transform.position, player.transform.position);

        if (distance < attackRange)
        {
            agent.isStopped = true;
            _nodeState = NodeState.SUCCESS;
        }
        else
        {
            agent.isStopped = false;
            _nodeState = NodeState.FAILURE;
        }

        return _nodeState;
    }
}
