using UnityEngine;

public class Attack : Node
{
    private readonly LifeManager playerLifeManager;
    private readonly float attackInterval;
    private readonly float damages;
    private float lastAttackTime;

    public Attack(LifeManager playerLifeManager, float attackInterval, float damages)
    {
        this.playerLifeManager = playerLifeManager;
        this.attackInterval = attackInterval;
        this.damages = damages;
        lastAttackTime = -attackInterval;
    }

    public override NodeState Evaluate()
    {
        if (Time.time < lastAttackTime + attackInterval)
            return NodeState.SUCCESS;

        playerLifeManager.TakeDamage(damages);
        lastAttackTime = Time.time;
        Debug.Log($"Enemy attacked player for {damages} damage");

        return NodeState.SUCCESS;
    }
}