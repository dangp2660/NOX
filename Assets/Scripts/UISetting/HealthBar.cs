using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider HealthSlider;
    public TMP_Text HealthBarText;
    private Damageable playerDamageable;
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerDamageable = player.GetComponent<Damageable>();
    }
    void Start()
    {
        HealthSlider.value = CaculateSliderPercentage(playerDamageable.CurrentHealth, playerDamageable.getMaxHealth());
        HealthBarText.text = $"{CaculateSliderPercentage(playerDamageable.CurrentHealth, playerDamageable.getMaxHealth()) * 100}";
    }

    // Update is called once per frame
    private float CaculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return playerDamageable.CurrentHealth / playerDamageable.getMaxHealth();
    }
    void Update()
    {
        
    }
    private void OnEnable()
    {
        playerDamageable.healthChanged.AddListener(OnPLaterHealthChange);
    }
    private void OnDisable()
    {
        playerDamageable.healthChanged.RemoveListener(OnPLaterHealthChange);
    }

    private void OnPLaterHealthChange(float newHealth, float maxHealth)
    {
        HealthSlider.value = CaculateSliderPercentage(newHealth, maxHealth);
        HealthBarText.text = $"{CaculateSliderPercentage(newHealth, maxHealth) * 100}";
    }
}
