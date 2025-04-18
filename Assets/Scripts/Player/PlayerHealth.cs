using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float currentHP;
    private Data data;
    private Animator animator;
    private bool isDead = false; // ✅ Thêm biến này

    [SerializeField] Collider2D playerCollider;

    public float getHP()
    {
        return currentHP;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Initialize(Data data)
    {
        this.data = data;
        currentHP = data.Hp;
        isDead = false; // reset trạng thái sống
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; 

        currentHP -= damage;
        Debug.Log("Player took " + damage + " damage. HP left: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger(AnimationStringList.Die);
        Debug.Log("Player died!");
        // TODO: Add animation, game over logic, etc.
    }

    public bool IsAlive() 
    {
        return !isDead;
    }
}
