using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Debug.Log("death" + gameObject.name);
    }
}
