using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public void Initialize(float maxLife)
    {
        maxHealth = maxLife;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Debug.Log("death " + gameObject.name);
        //Destroy(gameObject);
    }
}
