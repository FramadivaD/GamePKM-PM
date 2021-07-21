using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 0;
    [SerializeField] private float currentHealth = 0;

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            float valueHealth = Mathf.Clamp(value, 0, maxHealth);

            if (valueHealth > currentHealth) OnHealthIncreased?.Invoke();
            if (valueHealth < currentHealth) OnHealthDecreased?.Invoke();
            if (valueHealth <= 0 && currentHealth > 0) OnDied?.Invoke();

            OnHealthChanged?.Invoke();

            currentHealth = valueHealth;
        }
    }

    public bool IsDie
    {
        get
        {
            return currentHealth <= 0;
        }
    }

    public delegate void HealthEventHandler();
    public event HealthEventHandler OnHealthChanged;
    public event HealthEventHandler OnDied;
    public event HealthEventHandler OnHealthDecreased;
    public event HealthEventHandler OnHealthIncreased;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }
}
