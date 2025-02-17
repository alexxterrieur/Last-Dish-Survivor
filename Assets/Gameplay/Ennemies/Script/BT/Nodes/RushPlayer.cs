using UnityEngine;
using UnityEngine.AI;

public class RushPlayer : Node
{
    private NavMeshAgent agent;
    private GameObject player;

    public RushPlayer(NavMeshAgent agent, GameObject player)
    {
        this.agent = agent;
        this.player = player;
    }

    public override NodeState Evaluate()
    {
        agent.SetDestination(player.transform.position);

        _nodeState = NodeState.SUCCESS;
        return _nodeState;
    }
}
