using System.Collections;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    private CircleCollider2D bombCollider;
    private Rigidbody2D rb;
    private float bombDamage;
    public float explosionRadius;
    private float explosionTime;
    public float shakeDuration;
    public float shakeMagnitude;

    private CameraControler cameraController;
    [SerializeField] private SpriteRenderer explosionVisuel;


    public void Initialize(Vector2 direction, float speed, float damage, float explosionDelay, float bombSize)
    {
        bombCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        cameraController = Camera.main.GetComponent<CameraControler>();

        explosionVisuel.transform.localScale = new Vector3(explosionRadius * 1.2f, explosionRadius * 1.2f, explosionRadius * 1.2f);
        bombDamage = damage;
        explosionTime = explosionDelay;
        bombCollider.enabled = false;
        explosionRadius = bombSize;

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
        rb.linearVelocity = Vector3.zero;

        GetComponent<SpriteRenderer>().enabled = false;
        explosionVisuel.enabled = true;

        if (cameraController != null)
        {
            StartCoroutine(cameraController.CameraShake(shakeDuration, shakeMagnitude));
        }

        Destroy(gameObject, 1f);
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