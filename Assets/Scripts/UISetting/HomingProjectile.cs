using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float lifetime = 5f;

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
        if (rb != null)
        {
            rb.velocity = transform.right * speed;
        }

        Destroy(gameObject, lifetime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OneWay") || hasHit) return;

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
            anim.SetTrigger(AnimationStringList.Explode);
        }
    }

    public void OnDestroySelf()
    {
        Destroy(gameObject);
    }
}
