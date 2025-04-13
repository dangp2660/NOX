using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(DirectionTouch))]
public class PlayerMovement : MonoBehaviour
{
    private bool isMoving = false;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float airSpeed = 3f;

    private float currentWalkSpeed
    {
        get
        {
            if(CanMove)
            {
                if (IsMoving && !dt.IsOnWall)
                {
                    if (dt.IsGrounded)
                    {
                        if (IsRunning) return runSpeed;
                        return walkSpeed;
                    }
                    else return airSpeed;
                }
            }
             return 0f;
        }
    }

    private bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStringList.canMove);
        }
    }

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    private bool isRunning = false;
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

    private bool IsRunning
    {
        get => isRunning;
        set
        {
            isRunning = value;
            animator.SetBool(AnimationStringList.isRunning, value);
        }
    }

    private bool IsFacingRight
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dt = GetComponent<DirectionTouch>();
    }

    private void Update()
    {
        Move();
        animator.SetFloat(AnimationStringList.yVelocity, rb.velocity.y);
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

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
            IsRunning = true;
        else if (context.canceled)
            IsRunning = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && dt.IsGrounded)
        {
            animator.SetTrigger(AnimationStringList.Jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

}
