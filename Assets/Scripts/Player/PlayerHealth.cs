using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float currentHP;
    private Data data;

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
        Debug.Log("Player died!");
        // TODO: Add animation, game over logic, etc.
    }
}
