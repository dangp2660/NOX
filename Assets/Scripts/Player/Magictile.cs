using UnityEngine;

public class Magictile : MonoBehaviour
{
    [SerializeField] private Vector2 moveSpeed = new Vector2(2f, 0);
    [SerializeField] private float dame = 15f;

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;

    private bool hasHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        float direction = Mathf.Sign(transform.localScale.x);
        rb.velocity = new Vector2(moveSpeed.x * direction, moveSpeed.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.GetType());
        if (hasHit) return; // tránh trúng nhiều lần
        hasHit = true;
        if(hasHit)
        {
            Damageable damageable =  collision.GetComponent<Damageable>();
            if (damageable.isInvincible) return;
            damageable.TakeDamage(dame, 1);
        }
        
        // Dừng chuyển động và va chạm
        rb.velocity = Vector2.zero;
        col.enabled = false;

        // Play animation Impact
        if (anim != null)
        {
            anim.SetTrigger(AnimationStringList.Explode);
        }
           
    }
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
