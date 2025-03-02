using System;
using System.Collections;
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

    private SpriteRenderer spriteRenderer;
    private Coroutine damageCoroutine;

    private void Awake()
    {
        enemyInfo = GetComponent<EnemyInfo>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.2f);
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (gameObject.CompareTag("Player"))
        {
            healthBar.value = 1;
            targetHealth = 1;
            healthText.text = currentHealth.ToString();
        }
    }

    //private void Start()
    //{
    //    if (gameObject.CompareTag("Player"))
    //    {
    //        spriteRenderer = GetComponent<SpriteRenderer>();
    //        healthBar.value = 1;
    //        targetHealth = 1;

    //    }
    //}

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
            spriteRenderer.color = Color.red;
            targetHealth = currentHealth / maxHealth;
            healthText.text = currentHealth.ToString();

            if (damageCoroutine != null)
                StopCoroutine(damageCoroutine);

            damageCoroutine = StartCoroutine(SpriteRed());
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public enum TargetType
    {
        Player,
        Enemy,
        Both
    }

    public void ApplyDamageOverTime(float damagePerTick, float tickInterval, float duration, TargetType targetType)
    {
        if ((targetType == TargetType.Player && CompareTag("Player")) || (targetType == TargetType.Enemy && CompareTag("Enemy")) || (targetType == TargetType.Both))
        {
            StartCoroutine(DamageOverTimeCoroutine(damagePerTick, tickInterval, duration));
        }
    }


    private IEnumerator DamageOverTimeCoroutine(float damagePerTick, float tickInterval, float duration)
    {
        float elapsedTime = 0f;
        bool isTakingDoT = true;

        while (elapsedTime < duration && isTakingDoT)
        {
            if (currentHealth <= 0)
            {
                isTakingDoT = false;
                break;
            }

            TakeDamage(damagePerTick);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.yellow;
                yield return new WaitForSeconds(0.3f);
                spriteRenderer.color = Color.white;
            }

            elapsedTime += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }
    }



    private IEnumerator SpriteRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
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
                if (dropItem.isChest)
                {
                    Instantiate(dropItem.itemPrefab, transform.position, Quaternion.identity);
                }
                else if (UnityEngine.Random.value <= dropItem.dropChance)
                {
                    Instantiate(dropItem.itemPrefab, transform.position, Quaternion.identity);
                    break;
                }                
            }
        }
    }
}
