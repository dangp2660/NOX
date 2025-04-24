using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damegeable : MonoBehaviour
{
    [SerializeField] private Data Stats;
    [SerializeField] private float DameRate = 0.2f;
    private float currentHealth;
    private float currentAttack;
    private bool isAlive = true;
    private bool isInvincible = false;
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            if (currentHealth <= 0)
            {
                isAlive = false;
                animator.SetBool(AnimationStringList.Die, true);
            }
        }
    }
    private Animator animator;
    [SerializeField] private float timeScinceHit = 0;
    [SerializeField] private float invincibilityTime = 0.25f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = Stats.Hp;
        currentAttack = Stats.Dame;
    }
    private void Update()
    {
        if (isInvincible)
        {
            if(timeScinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeScinceHit = 0;
            }

            timeScinceHit += Time.deltaTime;
        }
    }

    public void TakeDame(float Dame)
    {
        if (!isAlive && isInvincible) return;
        currentHealth -= (currentAttack *  DameRate);   
        isInvincible = true;
    }
    public bool getIsAlive()
    {
        return isAlive;
    }
}
