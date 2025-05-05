using UnityEngine;

public class SpellCooldownManager : MonoBehaviour
{
    public static SpellCooldownManager Instance;

    public float cooldownDuration = 2f;
    private float cooldownTimer = 0f;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // nếu muốn giữ khi chuyển scene
        }
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public bool CanUseSpell()
    {
        return cooldownTimer <= 0f;
    }

    public void TriggerCooldown()
    {
        cooldownTimer = cooldownDuration;
    }

    public float GetRemainingCooldown()
    {
        return Mathf.Max(0f, cooldownTimer);
    }
}
