using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "KnifeWeapon", menuName = "Scriptable Objects/Weapon/Knife")]
public class KnifeWeapon : Weapon
{
    public int numberOfProjectiles;
    public float timeBetweenProjectiles;
    public float projectileSpeed;

    public override void Activate(GameObject user)
    {
        AttackHandler attackHandler = user.GetComponent<AttackHandler>();
        if (attackHandler != null)
        {
            attackHandler.StartAttack(user, abilityPrefab, numberOfProjectiles, timeBetweenProjectiles, cooldown, projectileSpeed, damage + damageBonus);
        }

        user.GetComponent<MonoBehaviour>().StartCoroutine(ReturnToPoolAfterDuration(lastInstance, duration));
    }

    private IEnumerator ReturnToPoolAfterDuration(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (instance.activeInHierarchy)
        {
            PoolingManager.Instance.ReturnToPool(instance.name, instance);
        }
    }
}
