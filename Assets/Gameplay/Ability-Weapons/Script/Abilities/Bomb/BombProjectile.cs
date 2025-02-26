using System.Collections;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    private Collider2D bombCollider;
    private Rigidbody2D rb;
    private float bombDamage;
    private float explosionTime;

    public void Initialize(Vector2 direction, float speed, float damage, float explosionDelay)
    {
        bombCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        bombDamage = damage;
        explosionTime = explosionDelay;
        bombCollider.enabled = false;

        rb.linearVelocity = direction.normalized * speed;

        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionTime);
        Explode();
    }

    private void Explode()
    {
        bombCollider.enabled = true;
        Destroy(gameObject, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<LifeManager>().TakeDamage(bombDamage);
            Explode();
        }
    }
}