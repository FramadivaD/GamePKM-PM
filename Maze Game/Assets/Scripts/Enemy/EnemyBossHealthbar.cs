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

    private void Start()
    {
        slider.minValue = 0;
        slider.maxValue = health.MaxHealth;

        health.OnHealthChanged += RefreshHealthBar;

        healthBarParent.SetActive(false);
    }

    private void Update()
    {
        if (enemyBoss.BossModeStarted)
        {
            // refresh health
            health.CurrentHealth = health.MaxHealth;
            healthBarParent.SetActive(true);
        }
    }

    void RefreshHealthBar()
    {
        slider.value = health.CurrentHealth;
    }
}
