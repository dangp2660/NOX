using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float currentHP;
    private Data data;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Initialize(Data data)
    {
        this.data = data;
        currentHP = data.Hp;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log("Player took " + damage + " damage. HP left: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger(AnimationStringList.Die);
        Debug.Log("Player died!");
        // TODO: Add animation, game over logic, etc.
    }
}
