using UnityEngine;

public class DirectionTouch : MonoBehaviour
{
    private BoxCollider2D colliderCool;
    [SerializeField] private ContactFilter2D contactFilter;
    private Animator animator;
    private Rigidbody2D rb;

    private RaycastHit2D[] hitResults = new RaycastHit2D[5];
    private RaycastHit2D[] wallHit = new RaycastHit2D[5];
    private RaycastHit2D[] ceilingHit = new RaycastHit2D[5];

    [SerializeField] private float distance = 0.05f;
    [SerializeField] private float wallDistance = 0.5f;
    [SerializeField] private float ceilingDistance = 0.05f;

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

    private Vector2 wallCheckDirection => transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    private void Awake()
    {
        colliderCool = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (colliderCool != null)
        {
            IsGrounded = colliderCool.Cast(Vector2.down, contactFilter, hitResults, distance) > 0;
            IsOnWall = colliderCool.Cast(wallCheckDirection, contactFilter, wallHit, wallDistance) > 0;
            IsOnCeiling = colliderCool.Cast(Vector2.up, contactFilter, ceilingHit, ceilingDistance) > 0;
        }
    }
}
