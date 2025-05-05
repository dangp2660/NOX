using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider healthSlider;
    public TMP_Text healthBarText;

    [Header("Target")]
    [SerializeField] private Damageable targetDamageable;

    private void Start()
    {
        if (targetDamageable != null)
        {
            UpdateHealthBar(targetDamageable.CurrentHealth, targetDamageable.getMaxHealth());
            targetDamageable.healthChanged.AddListener(OnHealthChanged);
        }
        else
        {
            Debug.LogWarning("No Damageable assigned to HealthBar on " + gameObject.name);
        }
    }

    private void OnDestroy()
    {
        if (targetDamageable != null)
        {
            targetDamageable.healthChanged.RemoveListener(OnHealthChanged);
        }
    }

    public void SetTarget(Damageable newTarget)
    {
        if (targetDamageable != null)
        {
            targetDamageable.healthChanged.RemoveListener(OnHealthChanged);
        }

        targetDamageable = newTarget;

        if (targetDamageable != null)
        {
            targetDamageable.healthChanged.AddListener(OnHealthChanged);
            UpdateHealthBar(targetDamageable.CurrentHealth, targetDamageable.getMaxHealth());
        }
    }

    private void OnHealthChanged(float newHealth, float maxHealth)
    {
        UpdateHealthBar(newHealth, maxHealth);
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float percent = currentHealth / maxHealth;
        healthSlider.value = percent;
        healthBarText.text = $"{Mathf.RoundToInt(percent * 100)}%";
    }
}
