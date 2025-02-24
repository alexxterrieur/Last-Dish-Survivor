using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject playerTarget;
    public LifeManager playerLifeManager;
    private EnemyInfo enemyInfo;

    public float speed;
    public float distance;

    public float attackInterval;
    public float attackDamage;

    private bool isAttacking = false;
    SpriteRenderer playerSprite;

    void Start()
    {
        playerTarget = GameObject.FindWithTag("Player");
        playerLifeManager = playerTarget.GetComponent<LifeManager>();
        enemyInfo = GetComponent<EnemyInfo>();

        InitializeStats();
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, playerTarget.transform.position);

        Vector2 direction = playerTarget.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime);
    }

    private void InitializeStats()
    {
        speed = enemyInfo.GetSpeed();
        attackInterval = enemyInfo.GetAttackInterval();
        attackDamage = enemyInfo.GetAttackDamage();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            isAttacking = true;
            //playerSprite = collision.gameObject.GetComponent<SpriteRenderer>();
            //playerSprite.color = Color.red;

            StartCoroutine(AttackPlayer());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = false;
            //playerSprite.color = Color.white;
            StopCoroutine(AttackPlayer());
        }
    }

    private IEnumerator AttackPlayer()
    {
        while (isAttacking)
        {
            playerLifeManager.TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackInterval);
        }
    }
}
