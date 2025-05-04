using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DarkEnergyBar : MonoBehaviour
{
    [SerializeField] private Slider darkEnergySlider;
    [SerializeField] private TextMeshProUGUI darkEnergyText;

    private DarkEnergyManager currentEnergyManager;

    private void Awake()
    {
        if (darkEnergySlider == null || darkEnergyText == null)
        {
            Debug.LogError("DarkEnergyBar components not assigned!", this);
            enabled = false;
        }
    }

    private void Start()
    {
        if (currentEnergyManager != null)
        {
            UpdateEnergyBar();
        }
    }

    private void UpdateEnergyBar()
    {
        if (currentEnergyManager != null)
        {
            float value = Calculate(currentEnergyManager.getDarkEnergy(), currentEnergyManager.getMaxEnergy());
            darkEnergySlider.value = value;
            darkEnergyText.text = $"{value * 100:F0}%";
        }
    }

    private float Calculate(float currentEnergy, float maxEnergy)
    {
        return maxEnergy > 0 ? currentEnergy / maxEnergy : 0;
    }

    private void OnPlayerEnergyChange(float newEnergy, float maxEnergy)
    {
        float value = Calculate(newEnergy, maxEnergy);
        darkEnergySlider.value = value;
        darkEnergyText.text = $"{value * 100:F0}%";
    }

    private void OnEnable()
    {
        if (currentEnergyManager != null)
        {
            currentEnergyManager.darkEnergyChange.AddListener(OnPlayerEnergyChange);
        }
    }

    private void OnDisable()
    {
        if (currentEnergyManager != null)
        {
            currentEnergyManager.darkEnergyChange.RemoveListener(OnPlayerEnergyChange);
        }
    }

    public void SwitchEnergyManager(DarkEnergyManager newEnergyManager)
    {
        if (newEnergyManager == null) return;

        if (currentEnergyManager != null)
        {
            currentEnergyManager.darkEnergyChange.RemoveListener(OnPlayerEnergyChange);
        }

        currentEnergyManager = newEnergyManager;
        currentEnergyManager.darkEnergyChange.AddListener(OnPlayerEnergyChange);
        UpdateEnergyBar();
    }
}