using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    [SerializeField] private EnemiesInfos enemiesScriptable;
    LifeManager lifeManager;

    private string enemyName;
    private float maxLife;
    private float attackRange;
    private float attackInterval;
    private float attackDamage;
    private float speed;

    private void Awake()
    {
        if (enemiesScriptable != null)
        {
            InitializeStats();
        }

        lifeManager = GetComponent<LifeManager>();
        if (lifeManager != null)
        {
            lifeManager.Initialize(maxLife);
        }
    }


    private void InitializeStats()
    {
        enemyName = enemiesScriptable.enemyName;
        maxLife = enemiesScriptable.maxLife;
        attackRange = enemiesScriptable.attackRange;
        attackInterval = enemiesScriptable.attackInterval;
        attackDamage = enemiesScriptable.attackDamage;
        speed = enemiesScriptable.speed;
    }

    public string GetEnemyName() => enemyName;
    public float GetMaxLife() => maxLife;
    public float GetAttackRange() => attackRange;
    public float GetAttackInterval() => attackInterval;
    public float GetAttackDamage() => attackDamage;
    public float GetSpeed() => speed;

    public List<DropItem> GetDropItems() => enemiesScriptable != null ? enemiesScriptable.dropItems : null;

}
