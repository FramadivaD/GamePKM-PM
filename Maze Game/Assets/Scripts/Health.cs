using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 0;
    [SerializeField] protected float currentHealth = 0;

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
    }

    public bool IsDie
    {
        get
        {
            return currentHealth <= 0;
        }
    }

    private void Start()
    {
        SetHealth(maxHealth);
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
    }

    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
    }
}
