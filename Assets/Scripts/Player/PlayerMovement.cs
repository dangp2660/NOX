using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(DirectionTouch))]
public class PlayerMovement : MonoBehaviour
{
    private bool isMoving = false;

    [Header("Speed move")]
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float fallMuiltiple = 2.5f;
    [SerializeField] private float lowMuiltiple = 2f;
    [SerializeField] private float airSpeed = 7.5f;
    [SerializeField] private float attackMove = 2f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCoolDown = 0.5f;
    [SerializeField] private GameObject GhostDash;
    [SerializeField] private float GhostDelay;

    [Header("Jump Advance")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private bool allowDoubleJump = true;

    private Coroutine DashGhost;
    private bool isDash = false;
    private bool canDash = true;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool canDoubleJump = true;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private bool isFacingRight = true;
    private DirectionTouch dt;

    private bool IsMoving
    {
        get => isMoving;
        set
        {
            isMoving = value;
            animator.SetBool(AnimationStringList.isMoving, value);
        }
    }

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

    private bool CanMove => animator.GetBool(AnimationStringList.canMove);

    private float currentWalkSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !dt.IsOnWall)
                {
                    if (dt.IsGrounded)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return airSpeed;
                    }
                }
            }
            return 0f;
        }
    }

    private bool IsDash
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
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dt = GetComponent<DirectionTouch>();
    }

    private void Update()
    {
        if (IsDash) return;

        Move();
        animator.SetFloat(AnimationStringList.yVelocity, rb.velocity.y);

        // Coyote time & double jump
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

        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            Jump();
            jumpBufferCounter = 0;
        }
        else if (jumpBufferCounter > 0 && canDoubleJump && allowDoubleJump)
        {
            Jump();
            canDoubleJump = false;
            jumpBufferCounter = 0;
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMuiltiple - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowMuiltiple - 1) * Time.fixedDeltaTime;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
            IsFacingRight = true;
        else if (moveInput.x < 0 && IsFacingRight)
            IsFacingRight = false;
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveInput.x * currentWalkSpeed, rb.velocity.y);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if (context.canceled && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void Jump()
    {
        animator.SetTrigger(AnimationStringList.Jump);

        float horizontalBoost = moveInput.x * runSpeed * airSpeed; // hoặc * airSpeed nếu bạn muốn xa hơn khi ở trên không
        rb.velocity = new Vector2(horizontalBoost, jumpSpeed);
    }


    public void OnDash(InputAction.CallbackContext context)
    {
        if (canDash && CanMove)
        {
            StartCoroutine(Dashing());
            dashEffect();
        }
    }

    IEnumerator Dashing()
    {
        canDash = false;
        IsDash = true;

        float dashDirection = IsFacingRight ? 1 : -1;

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

    private void dashEffect()
    {
        if (DashGhost != null)
        {
            StopCoroutine(DashGhost);
        }
        DashGhost = StartCoroutine(dashGhost());
    }

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
    public void AttackPush()
    {
        if (!dt.IsOnWall && CanMove)
        {
            float pushDir = IsFacingRight ? 1f : -1f;
            rb.velocity = new Vector2(pushDir * attackMove, rb.velocity.y);
        }
    }
}
