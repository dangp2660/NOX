using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private bool isMoving = true;
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

    // Update is called once per frame
    void Update()
    {
        if (!Damageable.IsAlive)
        {
            return;
        }
        handleMove();
        handleAttack();
        die();
    }
    private float getCurrientMoveSpeed()
    {
        if(!canMove) return 0f;
            return currentPhase == bossPhase.phase1 ? MovePhase1 : MovePhase2;
    }
    protected virtual void handleMove()
    {
        IsMoving = rb.velocity != Vector2.zero;
        if (IsMoving) 
        {
            Vector2 direction = (Player.transform.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * getCurrientMoveSpeed(), rb.velocity.y);
            if (direction.x > 0.001f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if(direction.x < -0.001f)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }
    protected virtual void handleAttack()
    {
        AttackTimer -= Time.deltaTime;  
        if (AttackTimer < 0f)
        {
            if (attackZone.detectedColliders.Count > 0 && !isAttack)
            {
                isAttack = true;
                AttackPlayer();
            }
        }        
    }
    
    protected virtual void AttackPlayer()
    {
        AttackTimer = AttackCoolDown;
        int moveSet = isPhase2() ? Random.Range(1, 4) : Random.Range(1, 3);
        Debug.Log(moveSet);
        switch (moveSet)
        {

            case 1:
                animator.SetTrigger(AnimationStringList.Attack1);
                StartCoroutine(DelayAttack());
                break;
            case 2:
                animator.SetTrigger(AnimationStringList.Attack2);
                StartCoroutine(DelayAttack());
                break;
            case 3:
                animator.SetTrigger(AnimationStringList.Attack3);
                StartCoroutine(DelayAttack());
                break;
        }
    }

    private bool isPhase2()
    {
        return currentPhase == bossPhase.phase2 ? true : false;
    }

    IEnumerator DelayAttack()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.5f);
        Vector2 direction = (Player.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * getCurrientMoveSpeed(), rb.velocity.y);
        isAttack = false;
    }
    
    protected virtual void changePhase()
    {
        if(Damageable.CurrentHealth <= 50 * Damageable.getMaxHealth())
        {
            currentPhase = bossPhase.phase2;
        }
    }

    protected virtual void die()
    {
        if (!Damageable.IsAlive)
        {
            Debug.Log("Boss die");
            animator.SetBool(AnimationStringList.isAlive, false);
        }
    }

    public void StartBossFight()
    {
        this.enabled = true;
        Debug.Log("Trận chiến với boss đã bắt đầu!");
        // Hiệu ứng âm thanh hoặc animation khi trận chiến bắt đầu
    }
}
