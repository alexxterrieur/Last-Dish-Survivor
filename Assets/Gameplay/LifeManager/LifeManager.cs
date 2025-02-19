using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    private EnemyInfo enemyInfo;

    private void Awake()
    {
        enemyInfo = GetComponent<EnemyInfo>();
    }

    public void Initialize(float maxLife)
    {
        maxHealth = maxLife;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        if(gameObject.tag != "Player")
        {
            if (enemyInfo != null)
            {
                TryDropItem();
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("death " + gameObject.name);
        }
    }

    private void TryDropItem()
    {
        List<DropItem> dropItems = enemyInfo.GetDropItems();

        if (dropItems != null && dropItems.Count > 0)
        {
            foreach (var dropItem in dropItems)
            {
                if (Random.value <= dropItem.dropChance)
                {
                    Instantiate(dropItem.itemPrefab, transform.position, Quaternion.identity);
                    break;
                }
            }
        }
    }

}
