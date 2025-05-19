using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMonster : MonoBehaviour
{
    [SerializeField] private Data monsterData;
    [SerializeField] private float moveSpeed = 2f;
    [Header("Block Setup")]
    [SerializeField] private float blockDuration = 5f;
    [SerializeField] private float blockCoolDown = 8;
    private float blockTimer = 0f;
    private Damageable damageable;
    private Animator animator;
    private bool isBlocking = false;
    private Transform Player;
    public bool IsBlocking
    {
        get
        {
            return isBlocking;
        }
        set
        {
            isBlocking = value;
            animator.SetBool(AnimationStringList.isBlocking, value);
        }
    }
    private bool isMoving = true;
    public bool IsMoving
    {
        get { return isMoving; }
        set
        {
            isMoving = value;
            animator.SetBool(AnimationStringList.isMoving, value);
        }
    }

    private bool canMove => animator.GetBool(AnimationStringList.canMove);
    private Rigidbody2D rb;
    private DetectionZone zone;
    private float direcrtionBlock = 3f;
    private bool Attack = false;
    [SerializeField] private float timeAttack;
    
    private void Awake()
    {
        zone = GetComponent<DetectionZone>();   
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private void Update()
    {
        // Tự động kích hoạt block khi cooldown kết thúc
        float direction = Vector2.Distance(transform.position, Player.position);
        if (blockTimer <= 0 && !isBlocking && direction <= direcrtionBlock)
        {
            startBlock();
        }


        // Quản lý thời gian block
        if (isBlocking)
        {
            blockTimer -= Time.deltaTime;
            if (blockTimer <= 0)
            {
                stopBlock();
            }
        }
        if (zone.detectedColliders.Count > 0) 
        {
            StartCoroutine(delayBlockToAttack(timeAttack));
        }

    }
    IEnumerator delayBlockToAttack(float time)
    {
        Attack = true;
        animator.SetBool(AnimationStringList.Attack, Attack);
        isBlocking = false;
        yield return new WaitForSeconds(time + 0.2f);
        Attack = false;
        isBlocking = true;
    }
    private void FixedUpdate()
    {
        handleMove();
    }

    public void handleMove()
    {
        if (!IsMoving) return;

        Vector2 direction = (Player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

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

    private void startBlock()
    {
        isBlocking = true;
        blockTimer = blockDuration;
    }

    private void stopBlock()
    {
        isBlocking = false;
        blockTimer = blockTimer;
    }
    public bool AttemptDamage(float damage, Vector2 attackPosition)
    {
        // Hướng từ quái tới điểm tấn công (player)
        Vector2 direction = (attackPosition - (Vector2)transform.position).normalized;
        // Hướng mặt quái (giả sử transform.localScale.x > 0 là hướng sang phải)
        Vector2 forward = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Kiểm tra xem tấn công có từ phía trước quái không (dot > 0)
        bool fromFront = Vector2.Dot(forward, direction) > 0;

        if (fromFront && isBlocking)
        {
            // Quái đang block đòn từ phía trước -> giảm sát thương
            float reducedDamage = Mathf.Max(damage *.5f, 0);
            damageable.TakeDamage(reducedDamage, 1f);  // Truyền damageRate=1f để tránh nhân lần nữa
            animator?.SetTrigger("BlockHit");
            Debug.Log($"Block thành công, nhận {reducedDamage} sát thương!");
            return false;  // Không nhận sát thương gốc nữa
        }
        else if (!fromFront)
        {
            // Tấn công từ phía sau, nhận sát thương đầy đủ
            damageable.TakeDamage(damage, 1f);
            animator?.SetTrigger("Hurt");
            Debug.Log($"Bị tấn công từ phía sau, nhận {damage} sát thương!");
            return true;
        }

        // Nếu tấn công từ phía trước nhưng quái không block
        Debug.Log("Tấn công phía trước nhưng không block được!");
        return true;  // Cho phép nhận sát thương bình thường
    }
}
