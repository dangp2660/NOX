using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum bossPhase
{
    phase1, phase2
}

public class BossBase : MonoBehaviour
{
    [Header("MoveSpeed")]
    [SerializeField] private float MovePhase1 = 5f;
    [SerializeField] private float MovePhase2 = 8f;

    [Header("Attack")]
    [SerializeField] private DetectionZone attackZone;
    [SerializeField] private float AttackCoolDown = 1f;
    private float AttackTimer = 0f;
    private bossPhase currentPhase;
    private Animator animator;
    private Transform Player;
    private Damageable Damageable;
    private Rigidbody2D rb;
    private bool isAttack = false;
    private bool canMove => animator.GetBool(AnimationStringList.canMove);
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();
        Player = GameObject.FindWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentPhase = bossPhase.phase1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Damageable.IsAlive)
        {
            return;
        }
        handleMove();
        handleAttack();
    }
    private float getCurrientMoveSpeed()
    {
        if(!canMove) return 0f;
        return currentPhase == bossPhase.phase1 ? MovePhase1 : MovePhase2;
    }
    protected virtual void handleMove()
    {
        Vector2 direction = (Player.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * getCurrientMoveSpeed(), rb.velocity.y);
    }

    protected virtual void handleAttack()
    {
        AttackTimer -= Time.deltaTime;  
        if (AttackTimer < 0f)
        {
            if (attackZone.detectedColliders.Count > 0 && !isAttack)
            {
                AttackPlayer();
            }
        }
        
    }
    
    protected virtual void AttackPlayer()
    {
        AttackTimer = AttackCoolDown;
        animator.SetTrigger(AnimationStringList.Attack1);
    }

}
