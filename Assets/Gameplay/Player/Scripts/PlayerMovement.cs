using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 move;
    public float speed;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isDashing = false;
    public float piercingDamage;
    public AbilityManager abilityManager;

    private Vector3 lastImagePos;
    [SerializeField] private float distanceBetweenImages;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        abilityManager = GetComponent<AbilityManager>();
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

    //Dash
    public void PerformDash(float dashDistance, float dashSpeed)
    {
        if (!isDashing)
        {
            StartCoroutine(DashCoroutine(dashDistance, dashSpeed));
        }
    }

    private IEnumerator DashCoroutine(float dashDistance, float dashSpeed)
    {
        isDashing = true;
        Vector2 dashDirection = move.normalized;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + dashDirection * dashDistance;

        float startTime = Time.time;
        float duration = dashDistance / dashSpeed;

        while (Time.time < startTime + duration)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, lastImagePos);
            if(Mathf.Abs(distance) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImagePos = transform.position;
            }


            yield return null;
        }

        isDashing = false;
    }

    //Piercing Dash
    public void PerformPiercingDash(float dashDistance, float dashSpeed, float damage)
    {
        if (!isDashing)
        {
            StartCoroutine(PiercingDashCoroutine(dashDistance, dashSpeed, damage));
        }
    }

    private IEnumerator PiercingDashCoroutine(float dashDistance, float dashSpeed, float damage)
    {
        isDashing = true;
        Vector2 dashDirection = move.normalized;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + dashDirection * dashDistance;

        piercingDamage = damage;
        Collider2D playerCollider = GetComponent<Collider2D>();
        playerCollider.isTrigger = true;

        float startTime = Time.time;
        float duration = dashDistance / dashSpeed;

        while (Time.time < startTime + duration)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);


            float distance = Vector3.Distance(transform.position, lastImagePos);
            if (Mathf.Abs(distance) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImagePos = transform.position;
            }

            yield return null;
        }

        playerCollider.isTrigger = false;
        isDashing = false;
    }

    //speedBonus
    public void PerformSpeedAbilityOverTime(Ability ability, float newSpeed, float duration)
    {
        StartCoroutine(SpeedOverTimeAbility(ability, newSpeed, duration));
    }

    private IEnumerator SpeedOverTimeAbility(Ability ability, float newSpeed, float duration)
    {
        float initialSpeed = speed;
        speed = newSpeed;

        WeaponsBonusUI.Instance.StartAbilityCooldownVisual(ability, duration, Color.red);

        float startTime = Time.time;
        while(Time.time < startTime + duration)
        {
            float distance = Vector3.Distance(transform.position, lastImagePos);
            if (Mathf.Abs(distance) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImagePos = transform.position;
            }

            yield return null;
        }
        
        speed = initialSpeed;

        abilityManager.SetCooldown(ability, ability.cooldown);
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
