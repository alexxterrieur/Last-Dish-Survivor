using UnityEngine;

[CreateAssetMenu(fileName = "HomingWeapon", menuName = "Scriptable Objects/Weapon/HomingWeapon")]
public class HomingWeapon : Weapon
{
    public float projectileSpeed;

    public override void Activate(GameObject user)
    {
        base.Activate(user);

        if (lastInstance == null) return;

        Transform target = FindClosestEnemy(user);
        if (target == null)
        {
            Debug.Log("Aucun ennemi trouvé !");
            return;
        }

        HomingProjectile homingProjectile = lastInstance.GetComponent<HomingProjectile>();
        if (homingProjectile != null)
        {
            homingProjectile.SetTarget(target, projectileSpeed);
            homingProjectile.damage = damage;
        }
    }

    private Transform FindClosestEnemy(GameObject user)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(user.transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }
}
