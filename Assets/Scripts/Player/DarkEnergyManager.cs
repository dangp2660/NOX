
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class DarkEnergyManager : MonoBehaviour
{
    [Header("Dark Energy Settings")]
    [SerializeField] private float maxDarkEnergy = 100f;
    private float currentDarkEnergy;

    [Header("Events")]
    public UnityEvent<float, float> darkEnergyChanged;

    private void Awake()
    {
        currentDarkEnergy = maxDarkEnergy;
    }

    public void UseDarkEnergy(float amount)
    {
        if (currentDarkEnergy >= amount)
        {
            currentDarkEnergy -= amount;
            darkEnergyChanged?.Invoke(currentDarkEnergy, maxDarkEnergy);
        }
    }

    public void RegenerateDarkEnergy(float amount)
    {
        currentDarkEnergy += amount;
        darkEnergyChanged?.Invoke(currentDarkEnergy, maxDarkEnergy);
    }
    public float CurrentDarkEnergy
    {
        get => currentDarkEnergy;
        set
        {
            currentDarkEnergy = Mathf.Max(0, value);
            darkEnergyChanged?.Invoke(currentDarkEnergy, maxDarkEnergy);
        }
    }
    public float MaxDarkEnergy => maxDarkEnergy;

    public void CopyDarkEnergy(DarkEnergyManager other)
    {
        this.currentDarkEnergy = other.currentDarkEnergy;
        darkEnergyChanged?.Invoke(currentDarkEnergy, maxDarkEnergy);
    }
}
