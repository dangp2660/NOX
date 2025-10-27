using System.Collections;
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
    private void Update()
    {
        Destroy(gameObject, 5f);
    }

    private void Start()
    {
            float direction = Mathf.Sign(transform.localScale.x);
            rb.velocity = new Vector2(moveSpeed.x * direction, moveSpeed.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            OnDestroy();
            return;
        }
        if (hasHit) return;
        hasHit = true;

        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable == null) damageable = collision.GetComponentInParent<Damageable>();
        if (damageable == null) damageable = collision.GetComponentInChildren<Damageable>();

        if (damageable != null && !damageable.isInvincible)
        {
            damageable.TakeDamage(dame, 1);
        }

        rb.velocity = Vector2.zero;
        col.enabled = false;

        if (anim != null)
        {
            anim.SetTrigger(AnimationStringList.Explode);
        }
    }
    public void onStop()
    {
        rb.velocity = Vector2.zero; 
    }
    public void startMove()
    {
        float direction = Mathf.Sign(transform.localScale.x);
        rb.velocity = new Vector2(moveSpeed.x * direction, moveSpeed.y);
        Destroy(gameObject, 5f);

    }
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
