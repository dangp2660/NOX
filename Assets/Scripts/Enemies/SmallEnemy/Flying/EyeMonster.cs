using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EyeMonster : Enemy
{
    [SerializeField] private DetectionZone wakeupZone;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform Player;
    private Rigidbody2D rb;
    private EnemyFollow follow;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        this.initialState = EnemyState.Sleep;
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        follow = GetComponent<EnemyFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Sleep:
                animator.SetBool(AnimationStringList.Wakeup, false);
                break;
            case EnemyState.Wakeup:
                WakeupState();
                break;
            case EnemyState.Attack:
                Attack();
                break;

        }

        if (wakeupZone.detectedColliders.Count > 0)
        {
            animator.SetBool(AnimationStringList.Wakeup, true);
            switchState(EnemyState.Wakeup);
        }
    }

    private void WakeupState()
    {
        float distance = Vector2.Distance(transform.position, Player.position);
        
        if (distance <= attackRange)
        {
            Debug.Log(distance);
            rb.velocity = Vector2.zero;
            switchState(EnemyState.Attack);
        }
        else
        {
            
            follow.handleMove();
        }
    }
    protected override void Attack()
    {
        animator.SetTrigger(AnimationStringList.Attack);
    }

}
