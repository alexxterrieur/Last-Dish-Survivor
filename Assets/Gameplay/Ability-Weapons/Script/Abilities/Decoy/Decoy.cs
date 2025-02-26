using System.Collections;
using UnityEngine;

public class Decoy : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float lifetime;
    private bool isActive = true;

    public void Initialize(Vector2 moveDirection, float moveSpeed, float lifeTime)
    {
        direction = moveDirection;
        speed = moveSpeed;
        lifetime = lifeTime;
        StartCoroutine(DestroyAfterTime());
    }

    private void Update()
    {
        if (isActive)
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyAI enemy = collision.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.SetTarget(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        isActive = false;
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
