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
    public float reduceDamageValue;
    public int respawnCount;

    private EnemyInfo enemyInfo;
    public static Action OnDeath;

    [Header("Health Bar UI")]
    private int healthToPrint;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private float fillSpeed;
    private float targetHealth;

    private SpriteRenderer spriteRenderer;
    private Coroutine damageCoroutine;

    //gameOver Player
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject respawnButton;

    private void Awake()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            enemyInfo = GetComponent<EnemyInfo>();
        }
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
        currentHealth -= damage * (1 - (reduceDamageValue / 100f));

        if (gameObject.CompareTag("Player"))
        {
            spriteRenderer.color = Color.red;
            targetHealth = currentHealth / maxHealth;
            healthToPrint = (int)currentHealth;
            healthText.text = healthToPrint.ToString();

            if (damageCoroutine != null)
                StopCoroutine(damageCoroutine);

            damageCoroutine = StartCoroutine(SpriteRed());
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    //--------------------------Damage Over Time (player ability)--------------------------------//
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

    //-------------------------------------------------------------------------------------------//

    public void Heal(float healthAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            targetHealth = currentHealth / maxHealth;
            healthToPrint = (int)currentHealth;
            healthText.text = healthToPrint.ToString();
        }
    }

    public void HealthOverTime(float healthAmount, float tickInterval)
    {
        StartCoroutine(HealthOverTimeCoroutine(healthAmount, tickInterval));
    }

    private IEnumerator HealthOverTimeCoroutine(float healthAmount, float tickInterval)
    {
        while (true)
        {
            Heal(healthAmount);
            yield return new WaitForSeconds(tickInterval);
        }
    }

    private void Death()
    {
        if (gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0f;

            gameOverMenu.SetActive(true);
            if(respawnCount > 0)
            {
                respawnButton.SetActive(true);
            }
            else
            {
                respawnButton.SetActive(false);
            }

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

    public void Respawn()
    {
        respawnCount--;

        currentHealth = maxHealth;
        Time.timeScale = 1f;
        gameOverMenu.SetActive(false);
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
