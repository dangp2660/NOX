using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    private Animator animator;
    public UnityEvent<float, float> healthChanged; 

    [Header("Stats")]
    [SerializeField] private Data stats; //dùng Data ScriptableObject

    private float maxHealth;
    private float currentHealth;
    private float defend;

    [Header("State")]
    [SerializeField] private bool isAlive = true;
    [SerializeField] public bool isInvincible = false;
    private float timeSinceHit = 0f;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get => isAlive;
        set
        {
            isAlive = value;
            Debug.Log("IsAlive set " + value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (stats == null)
        {
            Debug.LogError("Stats (Data) chưa được gán!", this);
        }

        currentHealth = stats.Hp;
        maxHealth = stats.Hp;
    }

    private void Update()
    {
        if (isInvincible)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
        }
        
    }

    public bool TakeDamage(float damage, float damageRate)
    {
        if (IsAlive && !isInvincible)
        {
            float totaldamage = damage * damageRate;
            if (defend > totaldamage) CurrentHealth -= 1;
            CurrentHealth -= (totaldamage - defend);
            isInvincible = true;
            return true;
        }
        return false;
    }

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Max(0, value);
            healthChanged?.Invoke(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                IsAlive = false;
            }
            else
            {
                IsAlive = true;
            }
        }
    }

    public float getMaxHealth() { return maxHealth; }
}
