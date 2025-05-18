using UnityEngine;

public class DirectionTouch : MonoBehaviour
{
    [Header("Collider Check")]
    [SerializeField] private Collider2D groundCollider; // Collider phụ để kiểm tra mặt đất
    [SerializeField] private LayerMask groundLayer;        // Layer mặt đất

    private Animator animator;

    private bool isGround;
    public bool IsGrounded
    {
        get => isGround;
        private set
        {
            isGround = value;
            if (animator != null)
                animator.SetBool(AnimationStringList.isGrounded, value);
        }
    }

    private bool isOnWall;
    public bool IsOnWall
    {
        get => isOnWall;
        private set
        {
            isOnWall = value;
            if (animator != null)
                animator.SetBool(AnimationStringList.isOnWall, value);
        }
    }

    private bool isOnCeiling;
    public bool IsOnCeiling
    {
        get => isOnCeiling;
        private set
        {
            isOnCeiling = value;
            if (animator != null)
                animator.SetBool(AnimationStringList.isOnCieling, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Kiểm tra mặt đất bằng collider phụ
        IsGrounded = groundCollider.IsTouchingLayers(groundLayer);


        // Kiểm tra va chạm tường
        Vector2 wallCheckDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D wallHit = Physics2D.Raycast(groundCollider.bounds.center, wallCheckDirection, 0.5f, groundLayer);
        IsOnWall = wallHit.collider != null;

        // Kiểm tra va chạm trần nhà
        RaycastHit2D ceilingHit = Physics2D.Raycast(groundCollider.bounds.center, Vector2.up, 0.1f, groundLayer);
        IsOnCeiling = ceilingHit.collider != null;
    }


}
