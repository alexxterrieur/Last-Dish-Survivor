using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    public float damage;

    public void SetTarget(Transform newTarget, float projectileSpeed)
    {
        target = newTarget;
        speed = projectileSpeed;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<LifeManager>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }    
}
