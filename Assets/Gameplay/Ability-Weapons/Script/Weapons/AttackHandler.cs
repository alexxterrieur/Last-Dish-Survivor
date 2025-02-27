using System.Collections;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public void StartAttack(GameObject user, GameObject projectilePrefab, int numberOfProjectiles, float timeBetweenProjectiles, float timeBeforeNextSalvo, float projectileSpeed, float damage)
    {
        StartCoroutine(ShootProjectileSalvo(user, projectilePrefab, numberOfProjectiles, timeBetweenProjectiles, timeBeforeNextSalvo, projectileSpeed, damage));
    }

    private IEnumerator ShootProjectileSalvo(GameObject user, GameObject projectilePrefab, int numberOfProjectiles, float timeBetweenProjectiles, float timeBeforeNextSalvo, float projectileSpeed, float damage)
    {
        yield return new WaitForSeconds(timeBeforeNextSalvo);

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Vector2 direction = GetPlayerDirection();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270;

            GameObject projectile = Instantiate(projectilePrefab, user.transform.position, Quaternion.Euler(0, 0, angle));

            //knife
            KnifeProjectile projectileScript = projectile.GetComponent<KnifeProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Initialize(direction, projectileSpeed, damage);
            }

            BombProjectile bombProjectile = projectile.GetComponent<BombProjectile>();
            if(bombProjectile != null)
            {
                bombProjectile.Initialize(direction, projectileSpeed, damage, 0.7f, bombProjectile.explosionRadius);
            }

            yield return new WaitForSeconds(timeBetweenProjectiles);
        }
    }

    private Vector2 GetPlayerDirection()
    {
        switch (PlayerInfos.Instance.currentDirection)
        {
            case PlayerInfos.PlayerDirection.Left: return Vector2.left;
            case PlayerInfos.PlayerDirection.Right: return Vector2.right;
            case PlayerInfos.PlayerDirection.Up: return Vector2.up;
            case PlayerInfos.PlayerDirection.Down: return Vector2.down;
            default: return Vector2.right;
        }
    }
}
