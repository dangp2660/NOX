using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Transform player;         // Gán player vào đây (trong Unity)
    [SerializeField] private float speed = 2f;         // Tốc độ di chuyển
    private Animator animator;
    private Enemy baseEnemy;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isMoving = false;
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
        set
        {
            isMoving = value;
            animator.SetBool(AnimationStringList.isMoving, value);
        }
    }

    private void Awake()
    {
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        baseEnemy = GetComponent<Enemy>();
    }
    public void handleMove()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;

        IsMoving = Mathf.Abs(direction.x) > 0.01f;

        // Flip sprite
        if (direction.x > 0.001f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < -0.001f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

}
