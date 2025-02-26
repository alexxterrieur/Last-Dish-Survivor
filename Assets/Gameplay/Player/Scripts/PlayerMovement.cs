using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 move;
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isDashing = false;
    public float piercingDamage;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetStats();
    }

    private void GetStats()
    {
        speed = GetComponent<PlayerInfos>().characterClass.speed;
    }

    private void Update()
    {
        if (!isDashing)
        {
            rb.linearVelocity = move;
        }

        if (move.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (move.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        PlayerInfos.Instance.UpdatePlayerDirection(move);
    }

    public void MovementPlayer(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        move = new Vector2(input.x, input.y) * speed;
    }

    public void PerformDash(float dashDistance, float dashSpeed)
    {
        if (!isDashing)
        {
            StartCoroutine(DashCoroutine(dashDistance, dashSpeed));
        }
    }

    public void PerformPiercingDash(float dashDistance, float dashSpeed, float damage)
    {
        if (!isDashing)
        {
            StartCoroutine(PiercingDashCoroutine(dashDistance, dashSpeed, damage));
        }
    }

    private IEnumerator DashCoroutine(float dashDistance, float dashSpeed)
    {
        isDashing = true;
        Vector2 dashDirection = move.normalized;
        float startTime = Time.time;

        while (Time.time < startTime + (dashDistance / dashSpeed))
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        isDashing = false;
    }

    private IEnumerator PiercingDashCoroutine(float dashDistance, float dashSpeed, float damage)
    {
        isDashing = true;
        Vector2 dashDirection = move.normalized;
        float startTime = Time.time;

        // Désactiver les collisions du joueur pour l'invincibilité temporaire
        Collider2D playerCollider = GetComponent<Collider2D>();
        playerCollider.enabled = false;

        while (Time.time < startTime + (dashDistance / dashSpeed))
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        // Réactiver les collisions
        playerCollider.enabled = true;
        isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDashing && collision.CompareTag("Enemy"))
        {
            LifeManager enemyLife = collision.GetComponent<LifeManager>();
            if (enemyLife != null)
            {
                enemyLife.TakeDamage(piercingDamage);
            }
        }
    }
}
