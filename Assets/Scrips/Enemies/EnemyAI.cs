using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject playerTarget;
    public LifeManager playerLifeManager;

    public float attackRange;
    public float attackInterval;
    public float attackDamage;

    public Node start;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        playerTarget = GameObject.FindWithTag("Player");
        playerLifeManager = playerTarget.GetComponent<LifeManager>();


        //BT
        RushPlayer rushPlayer = new RushPlayer(agent, playerTarget);
        CanAttack canAttack = new CanAttack(agent, playerTarget, attackRange);
        Attack attack = new Attack(playerLifeManager, attackInterval, attackDamage, agent, attackRange);

        Sequence sequence1 = new Sequence(new List<Node> { canAttack, attack });
        Selector selector1 = new Selector(new List<Node> { sequence1, rushPlayer });

        start = selector1;
    }

    void Update()
    {
        if (start != null)
        {
            start.Evaluate();
        }
    }
}