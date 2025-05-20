using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float lifetime = 5f;

    private Vector2 direction;
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;
    private bool hasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 targetDir = (player.transform.position - transform.position).normalized;
            direction = targetDir;
            rb.velocity = direction * speed;

            // Flip projectile theo hướng
            if (direction.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null && !damageable.isInvincible)
        {
            damageable.TakeDamage(damage, 1);
            hasHit = true;
        }

        rb.velocity = Vector2.zero;
        col.enabled = false;

        if (anim != null)
        {
            anim.SetTrigger("Explode");
        }
    }

    public void OnDestroySelf()
    {
        Destroy(gameObject);
    }
}
