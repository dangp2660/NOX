using UnityEngine;

public class Damageable : MonoBehaviour
{
    private Animator animator;

    [Header("Stats")]
    [SerializeField] private Data stats; //dùng Data ScriptableObject đã có

    private float currentHealth;
    private float defend;

    [Header("State")]
    [SerializeField] private bool isAlive = true;
    [SerializeField] private bool isInvincible = false;
    private float timeSinceHit = 0f;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get => isAlive;
        set
        {
            isAlive = value;
            animator.SetBool(AnimationStringList.isAlive, value);
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
        defend = stats.Defend;
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
            if (currentHealth <= 0)
            {
                IsAlive = false;
            }
        }
    }
}
