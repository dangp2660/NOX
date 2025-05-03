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
        base.Update();
        switch (enemyState)
        {
            case EnemyState.Sleep:
                animator.SetBool(AnimationStringList.Wakeup, false);
                break;
            case EnemyState.Wakeup:
                WakeupState();
                break;
            case EnemyState.Die:
                animator.SetBool(AnimationStringList.isAlive, false); break;
        }

        if (wakeupZone.detectedColliders.Count > 0)
        {
            animator.SetBool(AnimationStringList.Wakeup, true);
            switchState(EnemyState.Wakeup);
        }
    }
    private bool canMove => animator.GetBool(AnimationStringList.canMove);
    private void WakeupState()
    {
        float distance = Vector2.Distance(transform.position, Player.position);
        
        if (distance <= attackRange)
        {
            //Debug.Log(distance);
            rb.velocity = Vector2.zero;
            animator.SetTrigger(AnimationStringList.Attack);
        }
        else
        {
            if(canMove) 
                follow.handleMove();
        }
    }
    protected override void Attack()
    {
        
    }

}
