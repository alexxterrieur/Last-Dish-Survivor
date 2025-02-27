using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject playerTarget;
    private GameObject currentTarget;
    private LifeManager targetLifeManager;
    private EnemyInfo enemyInfo;

    public float speed;
    public float distance;

    public float attackInterval;
    public float attackDamage;

    private bool isAttacking = false;

    //public LifeManager playerLifeManager;

    SpriteRenderer playerSprite;

    void Start()
    {
        playerTarget = GameObject.FindWithTag("Player");
        currentTarget = playerTarget;
        targetLifeManager = playerTarget.GetComponent<LifeManager>();
        enemyInfo = GetComponent<EnemyInfo>();

        InitializeStats();
    }

    void Update()
    {
        if (currentTarget == null)
        {
            currentTarget = playerTarget;
        }

        transform.position = Vector2.MoveTowards(transform.position, currentTarget.transform.position, speed * Time.deltaTime);
    }

    public void SetTarget(GameObject newTarget)
    {
        currentTarget = newTarget;

        if (newTarget.CompareTag("Player"))
        {
            targetLifeManager = playerTarget.GetComponent<LifeManager>();
        }
        else
        {
            targetLifeManager = newTarget.GetComponent<LifeManager>();
        }
    }

    private IEnumerator AttackTarget()
    {
        while (isAttacking)
        {
            if (targetLifeManager != null)
            {
                targetLifeManager.TakeDamage(attackDamage);
            }
            yield return new WaitForSeconds(attackInterval);
        }
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
            targetLifeManager.TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackInterval);
        }
    }
}
