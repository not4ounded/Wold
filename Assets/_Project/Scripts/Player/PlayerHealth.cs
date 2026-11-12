using Game.Interfaces;
using UnityEngine;

/// <summary>
/// Класс здоровья игрока
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Настройки здоровья")]
    [SerializeField] private float maxHealth = 100f;
    public bool IsAlive {  get; private set; }
    [SerializeField] private HealthEventChannelSO OnHealthChanged;
    [SerializeField] private VoidEventChannelSO OnDeath;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        IsAlive = true;
    }

    /// <summary>
    /// м/д получения урона
    /// </summary>
    /// <param name="amount">Количество урона</param>
    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;
        if (amount <= 0) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        OnHealthChanged?.RaiseEvent(currentHealth, maxHealth);

        if (currentHealth <= 0) Die();
    }

    /// <summary>
    /// м/д смерти
    /// </summary>
    private void Die()
    {
        OnDeath?.RaiseEvent();
        gameObject.SetActive(false);
    }
}
