using System;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float speed = 10f;
    public float maxHealth = 100f;
    public float gravity = -9.8f;
    public HealthEventChannelSO OnEnemyHealthChanged;
    public VoidEventChannelSO OnEnemyDeath;

    
    private float currentHealth;
    private bool isGrounded;
    private bool isAlive;

    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;
        if (amount <= 0) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        OnEnemyHealthChanged?.RaiseEvent(currentHealth, maxHealth);

        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        OnEnemyDeath?.RaiseEvent();
    }
}
