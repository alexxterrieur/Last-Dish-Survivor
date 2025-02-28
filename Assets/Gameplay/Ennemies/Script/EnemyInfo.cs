using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    [SerializeField] private EnemiesInfos enemiesScriptable;
    LifeManager lifeManager;

    private SpriteRenderer spriteRenderer;
    private string enemyName;
    private float maxLife;
    private float attackRange;
    private float attackInterval;
    private float attackDamage;
    private float speed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //if (enemiesScriptable != null)
        //{
        //    InitializeStats();
        //}

        lifeManager = GetComponent<LifeManager>();
        if (lifeManager != null)
        {
            lifeManager.Initialize(maxLife);
        }
    }


    public void ApplyEnemyInfo(EnemiesInfos enemiesInfos)
    {
        if (enemiesInfos != null)
        {
            enemiesScriptable = enemiesInfos;

            enemyName = enemiesInfos.enemyName;
            maxLife = enemiesInfos.maxLife;
            spriteRenderer.sprite = enemiesInfos.sprite;
            attackRange = enemiesInfos.attackRange;
            attackInterval = enemiesInfos.attackInterval;
            attackDamage = enemiesInfos.attackDamage;
            speed = enemiesInfos.speed;

            if (lifeManager != null)
            {
                lifeManager.Initialize(maxLife);
            }
        }
    }


    public string GetEnemyName() => enemyName;
    public float GetMaxLife() => maxLife;
    public float GetAttackRange() => attackRange;
    public float GetAttackInterval() => attackInterval;
    public float GetAttackDamage() => attackDamage;
    public float GetSpeed() => speed;

    public List<DropItem> GetDropItems() => enemiesScriptable != null ? enemiesScriptable.dropItems : null;

}
