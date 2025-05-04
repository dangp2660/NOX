using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DarkEnergyBar : MonoBehaviour
{
    public Slider DarkEnergySlider;
    public TMP_Text DarkEnergyText;

    private DarkEnergyManager energyManager;
    private GameObject player;

    private void Awake()
    {
        FindAndAssignEnergyManager();
    }

    private void Start()
    {
        if (energyManager != null)
        {
            energyManager.darkEnergyChanged.AddListener(OnEnergyChanged);
            UpdateEnergyBar(energyManager.CurrentDarkEnergy, energyManager.MaxDarkEnergy);
        }
    }

    private void Update()
    {
        // Tự động cập nhật nếu player bị switch (ví dụ trong biến hình)
        GameObject newPlayer = GameObject.FindGameObjectWithTag("Player");
        if (newPlayer != null && newPlayer != player)
        {
            if (energyManager != null)
                energyManager.darkEnergyChanged.RemoveListener(OnEnergyChanged);

            player = newPlayer;
            energyManager = player.GetComponent<DarkEnergyManager>();

            if (energyManager != null)
            {
                energyManager.darkEnergyChanged.AddListener(OnEnergyChanged);
                UpdateEnergyBar(energyManager.CurrentDarkEnergy, energyManager.MaxDarkEnergy);
            }
        }
    }

    private void FindAndAssignEnergyManager()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            energyManager = player.GetComponent<DarkEnergyManager>();
        }
    }

    private void UpdateEnergyBar(float currentEnergy, float maxEnergy)
    {
        float percentage = CalculatePercentage(currentEnergy, maxEnergy);
        DarkEnergySlider.value = percentage;
        DarkEnergyText.text = $"{percentage * 100:F0}%";
    }

    private float CalculatePercentage(float current, float max)
    {
        return (max > 0) ? current / max : 0f;
    }

    private void OnEnergyChanged(float newEnergy, float maxEnergy)
    {
        UpdateEnergyBar(newEnergy, maxEnergy);
    }

    private void OnDisable()
    {
        if (energyManager != null)
        {
            energyManager.darkEnergyChanged.RemoveListener(OnEnergyChanged);
        }
    }
}
