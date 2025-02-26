using System.Collections;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public float damagePerTick;
    [SerializeField] private float tickInterval = 0.3f;
    public float duration;

    private bool isActive = true;

    private void Start()
    {
        StartCoroutine(DestroyAfterDuration());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isActive) return;

        if (collision.CompareTag("Enemy"))
        {
            LifeManager enemyLife = collision.GetComponent<LifeManager>();
            if (enemyLife != null)
            {
                StartCoroutine(ApplyDamageOverTime(enemyLife));
            }
        }
    }

    private IEnumerator ApplyDamageOverTime(LifeManager enemy)
    {
        while (enemy != null && isActive && enemy.gameObject.CompareTag("Enemy"))
        {
            enemy.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
    }

    private IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        isActive = false;
        Destroy(gameObject);
    }
}
