using System.Collections;
using UnityEngine;

public class WhipBehavior : MonoBehaviour
{
    public float cooldown;
    public float damage;
    public float colliderCooldown;

    private Collider2D[] colliders;
    private SpriteRenderer[] spriteRenderers;
    private bool isAttacking = false;
    private SpriteRenderer playerSprite;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>(true);
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);

        foreach (Collider2D col in colliders)
        {
            col.gameObject.SetActive(false);
        }
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.gameObject.SetActive(false);
        }
    }

    public void Initialize(GameObject player)
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                FlipWhip();
                yield return StartCoroutine(PerformAttack());
                yield return new WaitForSeconds(cooldown);
                isAttacking = false;
            }
            yield return null;
        }
    }

    private IEnumerator PerformAttack()
    {
        foreach (Collider2D col in colliders)
        {
            col.gameObject.SetActive(false);
        }
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.gameObject.SetActive(false);
        }

        // Activation séquentielle des colliders
        foreach (Collider2D col in colliders)
        {
            int index = System.Array.IndexOf(colliders, col); // Récupère l'index du collider
            col.gameObject.SetActive(true); // Active le collider
            spriteRenderers[index].gameObject.SetActive(true); // Active le sprite associé
            DealDamage(col); // Applique les dégâts
            yield return new WaitForSeconds(colliderCooldown); // Attente entre chaque activation de collider
            col.gameObject.SetActive(false); // Désactive le collider
            spriteRenderers[index].gameObject.SetActive(false); // Désactive le sprite
        }
    }

    private void DealDamage(Collider2D col)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(col.bounds.center, col.bounds.size, 0);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                LifeManager life = enemyCollider.GetComponent<LifeManager>();
                if (life != null)
                {
                    life.TakeDamage(damage);
                }
            }
        }
    }

    private void FlipWhip()
    {
        if (playerSprite != null)
        {
            bool isFacingRight = playerSprite.flipX;

            if (isFacingRight)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
