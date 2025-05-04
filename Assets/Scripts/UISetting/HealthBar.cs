using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider HealthSlider;
    public TMP_Text HealthBarText;
    private Damageable playerDamageable;

    private void Awake()
    {
        // Find the player object and get its Damageable component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerDamageable = player.GetComponent<Damageable>();
    }

    void Start()
    {
        // Initialize health slider and text
        UpdateHealthBar(playerDamageable.CurrentHealth, playerDamageable.getMaxHealth());
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float healthPercentage = CalculateSliderPercentage(currentHealth, maxHealth);
        HealthSlider.value = healthPercentage;
        HealthBarText.text = $"{healthPercentage * 100}%";
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnEnable()
    {
        // Subscribe to health change events
        playerDamageable.healthChanged.AddListener(OnPlayerHealthChange);
    }

    private void OnDisable()
    {
        // Unsubscribe from health change events
        playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChange);
    }

    private void OnPlayerHealthChange(float newHealth, float maxHealth)
    {
        // Update health bar when health changes
        UpdateHealthBar(newHealth, maxHealth);
    }
}
