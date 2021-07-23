using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBossHealthbar : MonoBehaviour
{
    [SerializeField] private GameObject healthBarParent;
    [SerializeField] private Slider slider;

    public Health health;
    public EnemyBoss enemyBoss;

    private bool healthStarted = false;

    private void Start()
    {
        slider.minValue = 0;
        slider.maxValue = health.MaxHealth;

        health.OnHealthChanged += RefreshHealthBar;
        health.OnDied += OnDie;

        healthBarParent.SetActive(false);
    }

    private void OnDestroy()
    {
        health.OnHealthChanged -= RefreshHealthBar;
        health.OnDied -= OnDie;
    }

    private void Update()
    {
        if (!healthStarted)
        {
            if (enemyBoss.BossModeStarted)
            {
                // refresh health
                health.CurrentHealth = health.MaxHealth;
                healthBarParent.SetActive(true);

                healthStarted = true;
            }
        }
    }

    void RefreshHealthBar()
    {
        slider.value = health.CurrentHealth;
    }

    void OnDie()
    {
        healthBarParent.SetActive(false);
    }
}
