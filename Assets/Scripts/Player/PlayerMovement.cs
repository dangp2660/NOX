using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Yêu cầu các component bắt buộc phải có
[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(DirectionTouch))]
public class PlayerMovement : MonoBehaviour
{
    // Biến kiểm tra nhân vật có đang di chuyển không
    private bool isMoving = false;
    [Header("Speed move")]
    [SerializeField] private float runSpeed = 5f;         // Tốc độ chạy trên mặt đất
    [SerializeField] private float jumpSpeed = 5f;        // Lực nhảy
    [SerializeField] private float airSpeed = 7.5f;       // Tốc độ di chuyển trên không
    [SerializeField] private float attackMove = 0.8f;       // Lực đẩy khi tấn công
    [SerializeField] private float horizontalBoost = 10f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;       // Tốc độ dash
    [SerializeField] private float dashDuration = 0.2f;   // Thời gian dash
    [SerializeField] private float dashCoolDown = 0.5f;   // Thời gian hồi dash
    [SerializeField] private GameObject GhostDash;        // Prefab bóng mờ khi dash
    [SerializeField] private float GhostDelay;            // Khoảng thời gian spawn bóng mờ

    [Header("Jump Advance")]
    [SerializeField] private float coyoteTime = 0.2f;     // Thời gian "lì" cho phép nhảy ngay sau khi rớt khỏi mặt đất
    [SerializeField] private bool allowDoubleJump = true; // Có cho phép nhảy hai lần không
    [SerializeField] private float lowJumpMultiplier = 2f;
    // Các biến nội bộ phục vụ dash, nhảy
    private Coroutine DashGhost;
    private bool isDash = false;
    private bool canDash = true;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool canDoubleJump = true;

    // Các component
    private Damageable damageable;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private bool isFacingRight = true;
    private DirectionTouch dt; // Kiểm tra va chạm (ground, wall)

    // Trạng thái sống/chết
    public bool isAlive
    {
        get { return animator.GetBool(AnimationStringList.isAlive); }
    }

    // Quản lý trạng thái di chuyển
    private bool IsMoving
    {
        get => isMoving;
        set
        {
            isMoving = value;
            animator.SetBool(AnimationStringList.isMoving, value);
        }
    }

    // Quản lý hướng quay mặt nhân vật
    public bool IsFacingRight
    {
        get => isFacingRight;
        set
        {
            if (isFacingRight != value)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            isFacingRight = value;
        }
    }

    // Chỉ cho phép di chuyển nếu animation cho phép
    private bool CanMove => animator.GetBool(AnimationStringList.canMove);

    // Tính tốc độ hiện tại dựa vào trạng thái
    private float currentWalkSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !dt.IsOnWall)
                {
                    if (dt.IsGrounded)
                        return runSpeed;
                    else
                        return airSpeed;
                }
            }
            return isAlive? attackMove :0;
        }
    }
    private float CurrentSpeed;

    // Quản lý trạng thái dash
    public bool IsDash
    {
        get => isDash;
        set
        {
            isDash = value;
            animator.SetBool(AnimationStringList.isDash, value);
        }
    }

    private void Awake()
    {
        // Lấy các component

        damageable = GetComponent<Damageable>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dt = GetComponent<DirectionTouch>();
    }
    private void Update()
    {
        if (IsDash) return;

        Move();
        animator.SetFloat(AnimationStringList.yVelocity, rb.velocity.y);

        // Xử lý coyote time và double jump
        if (dt.IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            canDoubleJump = true;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        jumpBufferCounter -= Time.deltaTime;

        // Nếu bấm nhảy trong coyote time hoặc double jump
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            Jump();
            jumpBufferCounter = 0;
        }
        else if (jumpBufferCounter > 0 && canDoubleJump && allowDoubleJump)
        {
            canDoubleJump = false;
            Jump();
            jumpBufferCounter = 0;
        }
        
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

    }

    // Hàm được Input System gọi khi có tín hiệu di chuyển
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (isAlive)
        {
            
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    // Xử lý hướng quay mặt (flip)
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
            IsFacingRight = true;
        else if (moveInput.x < 0 && IsFacingRight)
            IsFacingRight = false;
    }

    // Hàm xử lý di chuyển
    private void Move()
    {
        rb.velocity = new Vector2(moveInput.x * currentWalkSpeed, rb.velocity.y);
    }

    // Hàm được Input System gọi khi nhấn Jump
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && CanMove)
        {
            jumpBufferCounter = 0.1f; // Nhảy trong khoảng 0.1s sau khi nhấn
        }
    }

    // Hàm nhảy (kèm theo lực đẩy ngang)
    private void Jump()
    {
        
        animator.SetTrigger(AnimationStringList.Jump);
        rb.velocity = new Vector2(horizontalBoost + rb.velocity.x, !canDoubleJump? 0.8f * jumpSpeed : jumpSpeed);
        //Debug.Log(rb.velocity);
    }

    // Hàm được Input System gọi khi nhấn Dash
    public void OnDash(InputAction.CallbackContext context)
    {
        if (canDash)
        {
            StartCoroutine(Dashing());
            dashEffect();
            //Debug.Log(damageable.isInvincible);
        }
    }

    // Coroutine thực hiện dash
    IEnumerator Dashing()
    {
        canDash = false;
        IsDash = true;
        float dashDirection = IsFacingRight ? 1 : -1;
        damageable.isInvincible = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dashSpeed * dashDirection, 0f);
        yield return new WaitForSeconds(dashDuration); 
        rb.velocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        IsDash = false;

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    // Kích hoạt hiệu ứng bóng mờ khi dash
    private void dashEffect()
    {
        if (DashGhost != null)
        {
            StopCoroutine(DashGhost);
        }
        DashGhost = StartCoroutine(dashGhost());
    }

    // Coroutine spawn bóng mờ trong lúc đang dash
    IEnumerator dashGhost()
    {
        while (IsDash)
        {
            GameObject ghost = Instantiate(GhostDash, transform.position, transform.rotation);
            Vector3 ghostScale = ghost.transform.localScale;
            ghostScale.x = IsFacingRight ? Mathf.Abs(ghostScale.x) : -Mathf.Abs(ghostScale.x);
            ghost.transform.localScale = ghostScale;
            Destroy(ghost, 0.5f);
            yield return new WaitForSeconds(GhostDelay);
        }
    }
    public float getYVelocity()
    {
        return rb.velocity.y;
    }

    public void CopyStateFrom()
    {
        this.isDash = false;
        this.canDash = true;
    }

}
