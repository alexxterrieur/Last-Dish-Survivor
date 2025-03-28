using UnityEngine;

public class KnifeProjectile : MonoBehaviour
{
    public float damage;
    public float speed;
    private Vector2 direction;

    public void Initialize(Vector2 dir, float projectileSpeed, float dmg)
    {
        direction = dir.normalized;
        speed = projectileSpeed;
        damage = dmg;
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<LifeManager>().TakeDamage(damage);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        PoolingManager.Instance.ReturnToPool(gameObject.name, gameObject);
    }
}
