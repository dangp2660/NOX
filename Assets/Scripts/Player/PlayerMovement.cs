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
    [SerializeField] private float airSpeed = 7.5f;
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCoolDown = 0.5f;

    private bool isDash = false;
    private bool canDash = true;
    private bool IsDash
    {
        get { return isDash; }
        set
        {
            isDash = value;
            animator.SetBool(AnimationStringList.isDash, value);

        }
    }

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
                        return runSpeed;
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
        if (IsDash) return;
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

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && dt.IsGrounded)
        {
            animator.SetTrigger(AnimationStringList.Jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        Debug.Log(canDash);

        if (canDash && CanMove)
        {
            StartCoroutine(Dashing());
        }
        
    }

    IEnumerator Dashing()
    {

        canDash = false;
        IsDash = true;
        float dashDirection = IsFacingRight ? 1 : -1;
        rb.velocity = new Vector2(dashSpeed * dashDirection, rb.velocity.y);
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = new Vector2(0, rb.velocity.y);
        IsDash = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}
