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

        health.OnDied += OnDie;

        healthBarParent.SetActive(false);
    }

    private void OnDestroy()
    {
        health.OnDied -= OnDie;
    }

    private void Update()
    {
        if (enemyBoss.BossModeStarted)
        {
            CheckBossEnemyPlayerTeam();
            if (!healthStarted)
            {
                health.CurrentHealth = health.MaxHealth;
                slider.value = health.MaxHealth;

                healthBarParent.SetActive(true);

                healthStarted = true;
            }

            RefreshHealthBar();
        }
    }

    void RefreshHealthBar()
    {
        slider.value = Mathf.Lerp(slider.value, health.CurrentHealth, 0.2f);
    }

    void OnDie()
    {
        healthBarParent.SetActive(false);
    }

    private void CheckBossEnemyPlayerTeam()
    {
        if (PhotonNetwork.connected)
        {
            if (!PhotonNetwork.player.IsMasterClient)
            {
                // if is client and enemy boss differ from team then disabl healthbar ui
                if (enemyBoss.TeamType != TeamHelper.FromPhotonTeam(PhotonNetwork.player.GetTeam()))
                {
                    healthBarParent.SetActive(false);
                }
            }
        }
    }
}
