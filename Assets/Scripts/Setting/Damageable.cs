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
            float totalDamage = damage * damageRate;

            // Kiểm tra nếu là quái và có khả năng block
            BlockMonster blockMonster = GetComponent<BlockMonster>();
            if (blockMonster != null)
            {
                Vector2 attackPosition = GameObject.FindWithTag("Player").transform.position;
                if (!blockMonster.AttemptDamage(totalDamage, attackPosition))
                {
                    return false;
                }
            }

            // Thực thi khi không block
            CurrentHealth -= totalDamage;
            Debug.Log($"{gameObject.name} nhận sát thương: {totalDamage}");
            healthChanged.Invoke(currentHealth, maxHealth);
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
                
                animator.SetTrigger(AnimationStringList.hurt);
                IsAlive = true;
            }
        }
    }

    public float getMaxHealth() { return maxHealth; }
    public void setMaxHP(float amount) => maxHealth = amount;
    public void upHP(float HP)
    {
        maxHealth += HP;
    }
    public void healthCopy(Damageable other)
    {
        this.currentHealth = other.currentHealth;
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        IsAlive = true;
        isInvincible = false;
        timeSinceHit = 0;
        healthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"{gameObject.name} HP reset to max: {maxHealth}");
    }

}