using System.Collections;
using UnityEngine;

public class BoomerangProjectile : MonoBehaviour
{
    public float damage;
    public float speed;
    public float maxDistance;
    public float timeBeforeReturn;

    private Vector2 direction;
    private Transform player;
    private bool isReturning = false;
    private Vector2 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        direction = Random.insideUnitCircle.normalized;
    }

    public void Initialize(Transform playerTransform, float maxDist, float returnTime, float moveSpeed, float dmg)
    {
        player = playerTransform;
        maxDistance = maxDist;
        timeBeforeReturn = returnTime;
        speed = moveSpeed;
        damage = dmg;

        StartCoroutine(MoveBoomerang());
    }

    private IEnumerator MoveBoomerang()
    {
        float traveledDistance = 0f;
        float timeElapsed = 0f;

        while (!isReturning)
        {
            if (traveledDistance >= maxDistance || timeElapsed >= timeBeforeReturn)
            {
                isReturning = true;
            }

            // Déplacer le boomerang dans sa direction actuelle
            transform.Translate(direction * speed * Time.deltaTime);
            traveledDistance = Vector2.Distance(startPosition, transform.position);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Retour vers le joueur
        while (isReturning)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            transform.Translate(directionToPlayer * speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.position) < 0.2f)
            {
                Destroy(gameObject);
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<LifeManager>().TakeDamage(damage);
        }
    }
}
