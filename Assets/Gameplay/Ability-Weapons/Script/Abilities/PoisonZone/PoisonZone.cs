using UnityEngine;
using static LifeManager;

public class PoisonZone : MonoBehaviour
{
    public float damagePerTick;
    public float tickInterval = 1f;
    public float effectDuration;
    public TargetType targetType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LifeManager lifeManager = collision.GetComponent<LifeManager>();
        if (lifeManager != null)
        {
            lifeManager.ApplyDamageOverTime(damagePerTick, tickInterval, effectDuration, targetType);
        }
    }
}
