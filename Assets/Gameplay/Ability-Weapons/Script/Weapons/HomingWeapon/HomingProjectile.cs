using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float damage;
    public float speed;
    private Transform target;

    public void SetTarget(Transform newTarget, float newSpeed)
    {
        target = newTarget;
        speed = newSpeed;
    }

    private void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            ReturnToPool();
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<LifeManager>()?.TakeDamage(damage);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        PoolingManager.Instance.ReturnToPool(gameObject.name, gameObject);
    }
}
