using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "MultiHomingWeapon", menuName = "Scriptable Objects/Weapon/MultiHomingWeapon")]
public class MultiHomingWeapon : Weapon
{
    public float projectileSpeed;
    public int maxTargets;

    public override void Activate(GameObject user)
    {
        base.Activate(user);

        List<Transform> targets = FindClosestEnemies(user, maxTargets);
        if (targets.Count == 0)
        {
            Debug.Log("No enemies found");
            return;
        }

        foreach (Transform target in targets)
        {
            GameObject instance = Instantiate(abilityPrefab, user.transform.position, Quaternion.identity);
            HomingProjectile homingProjectile = instance.GetComponent<HomingProjectile>();

            if (homingProjectile != null)
            {
                homingProjectile.SetTarget(target, projectileSpeed);
                homingProjectile.damage = damage + damageBonus;
            }
        }
    }

    private List<Transform> FindClosestEnemies(GameObject user, int maxCount)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.OrderBy(enemy => Vector3.Distance(user.transform.position, enemy.transform.position)).Take(maxCount).Select(enemy => enemy.transform).ToList();
    }
}