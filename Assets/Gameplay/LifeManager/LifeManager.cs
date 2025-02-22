using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    private EnemyInfo enemyInfo;
    public static Action OnDeath;

    [Header("Health Bar UI")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private float fillSpeed;
    private float targetHealth;

    private void Awake()
    {
        enemyInfo = GetComponent<EnemyInfo>();
    }

    private void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            healthBar.value = 1;
            targetHealth = 1;
        }
    }

    private void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            healthBar.value = Mathf.Lerp(healthBar.value, targetHealth, fillSpeed * Time.deltaTime);
        }
    }

    public void Initialize(float maxLife)
    {
        maxHealth = maxLife;
        currentHealth = maxHealth;

        if (gameObject.CompareTag("Player"))
        {
            healthBar.value = 1;
            targetHealth = 1;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (gameObject.CompareTag("Player"))
        {
            targetHealth = currentHealth / maxHealth;
            healthText.text = currentHealth.ToString();
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("death " + gameObject.name);
        }
        else
        {
            if (enemyInfo != null)
            {
                TryDropItem();
            }

            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    private void TryDropItem()
    {
        List<DropItem> dropItems = enemyInfo.GetDropItems();

        if (dropItems != null && dropItems.Count > 0)
        {
            foreach (var dropItem in dropItems)
            {
                if (UnityEngine.Random.value <= dropItem.dropChance)
                {
                    Instantiate(dropItem.itemPrefab, transform.position, Quaternion.identity);
                    break;
                }
            }
        }
    }
}
