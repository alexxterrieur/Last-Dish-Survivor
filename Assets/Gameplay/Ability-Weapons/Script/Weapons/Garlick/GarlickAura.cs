using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlickAura : MonoBehaviour
{
    public float damage;
    public float cooldown;
    private HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();

    private SpriteRenderer sprite;
    public float activeOpacity = 210f / 255f;
    public float inactiveOpacity = 120f / 255f;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        SetSpriteOpacity(inactiveOpacity);

        StartCoroutine(ApplyDamage());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.gameObject);
        }
    }

    private IEnumerator ApplyDamage()
    {
        while (true)
        {
            SetSpriteOpacity(210f / 255f);
            yield return new WaitForSeconds(0.5f);

            List<GameObject> enemiesToDamage = new List<GameObject>(enemiesInRange);

            foreach (GameObject enemy in enemiesToDamage)
            {
                if (enemy != null)
                {
                    LifeManager lifeManager = enemy.GetComponent<LifeManager>();
                    if (lifeManager != null)
                    {
                        lifeManager.TakeDamage(damage);
                    }
                }
            }


            yield return new WaitForSeconds(1f);

            SetSpriteOpacity(120f / 255f);

            yield return new WaitForSeconds(cooldown - 1.5f);
        }
    }

    private void SetSpriteOpacity(float opacity)
    {
        if (sprite != null)
        {
            Color color = sprite.color;
            color.a = opacity;
            sprite.color = color;
        }
    }

}
