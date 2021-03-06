using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : Health
{
    public Image[] hearts;

    private void Start()
    {
        OnHealthChanged += RefreshHealthUI;

        CurrentHealth = MaxHealth;
    }

    private void OnDestroy()
    {
        OnHealthChanged -= RefreshHealthUI;
    }

    private void RefreshHealthUI()
    {
        int healthCount = Mathf.FloorToInt(MaxHealth);
        for (int i = 0; i < healthCount || i < hearts.Length; i++)
        {
            if (i < hearts.Length)
            {
                if (i < CurrentHealth)
                {
                    EnableHeartIndex(i);
                }
                else
                {
                    DisableHeartIndex(i);
                }
            } else
            {
                DisableHeartIndex(i);
            }
        }
    }

    private void EnableHeartIndex(int index)
    {
        hearts[index].color = new Color(1f, 1f, 1f, 1f);
    }

    private void DisableHeartIndex(int index)
    {
        hearts[index].color = new Color(1f, 1f, 1f, 0.2353f);
    }
}
