using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private bool isActive = true;

    private List<EnemyAI> enemiesTargetingMe = new List<EnemyAI>();

    public void Initialize(Vector2 moveDirection, float moveSpeed)
    {
        isActive = true;

        direction = moveDirection;
        speed = moveSpeed;
        enemiesTargetingMe.Clear();
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
                enemiesTargetingMe.Add(enemy);
            }
        }
    }

    private void OnDisable()
    {
        isActive = false;

        foreach (EnemyAI enemy in enemiesTargetingMe)
        {
            if (enemy != null)
            {
                enemy.ResetTarget();
            }
        }

        enemiesTargetingMe.Clear();
    }
}
