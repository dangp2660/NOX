using UnityEngine;
using UnityEngine.Events;

public class DarkEnergyManager : MonoBehaviour
{
    [Header("Dark Form Settings")]
    [SerializeField] private float maxDarkEnergy = 100f;
    [SerializeField] private float energyCostPerSecond = 10f;
    [SerializeField] private float energyRegenRate = 5f;

    private float darkEnergy;
    private bool isDraining = false;

    public UnityEvent<float, float> darkEnergyChange = new UnityEvent<float, float>();

    private void Start()
    {
        darkEnergy = maxDarkEnergy;
        darkEnergyChange?.Invoke(darkEnergy, maxDarkEnergy);
    }

    private void Update()
    {
        if (isDraining)
        {
            darkEnergy -= energyCostPerSecond * Time.deltaTime;
            darkEnergy = Mathf.Max(darkEnergy, 0f);
            darkEnergyChange?.Invoke(darkEnergy, maxDarkEnergy);

            if (darkEnergy <= 0f)
            {
                StopDrain();
            }
        }
        else
        {
            if (darkEnergy < maxDarkEnergy)
            {
                darkEnergy += energyRegenRate * Time.deltaTime;
                darkEnergy = Mathf.Min(darkEnergy, maxDarkEnergy);
                darkEnergyChange?.Invoke(darkEnergy, maxDarkEnergy);
            }
        }
    }

    public void StartDrain()
    {
        isDraining = true;
    }

    public void StopDrain()
    {
        isDraining = false;
    }

    public bool HasEnoughEnergy(float threshold)
    {
        return darkEnergy >= threshold;
    }

    public void RestoreEnergy(float amount)
    {
        darkEnergy = Mathf.Min(darkEnergy + amount, maxDarkEnergy);
        darkEnergyChange?.Invoke(darkEnergy, maxDarkEnergy);
    }

    public float getDarkEnergy() => darkEnergy;
    public float getMaxEnergy() => maxDarkEnergy;

    public void changeEnergy(DarkEnergyManager other)
    {
        darkEnergy = other.getDarkEnergy();
        darkEnergyChange?.Invoke(darkEnergy, maxDarkEnergy);
    }
}