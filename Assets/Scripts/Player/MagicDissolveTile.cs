using System.Collections;
using UnityEngine;

public class MagicDissolveTile : MonoBehaviour
{
    [SerializeField] private Vector2 moveSpeed = new Vector2(2f, 0);
    [SerializeField] private float damage = 15f;
    [SerializeField] private GameObject Warning;
    [SerializeField] private float dissolveTime = 1.5f;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;

    private bool hasHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (Warning != null)
        {
            StartCoroutine(WarningAndMove());
        }
        else
        {
            StartMove();
        }
    }

    private IEnumerator WarningAndMove()
    {
        Warning.SetActive(true);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(2f);
        Warning.SetActive(false);
        StartMove();
    }

    private void StartMove()
    {
        float direction = Mathf.Sign(transform.localScale.x);
        rb.velocity = new Vector2(moveSpeed.x * direction, moveSpeed.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OneWay")) return;
        if (hasHit) return;
        hasHit = true;

        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable == null) damageable = collision.GetComponentInParent<Damageable>();
        if (damageable == null) damageable = collision.GetComponentInChildren<Damageable>();

        if (damageable != null && !damageable.isInvincible)
        {
            damageable.TakeDamage(damage, 1);
        }

        rb.velocity = Vector2.zero;
        col.enabled = false;
        StartCoroutine(DissolveAndDestroy());
    }

    private IEnumerator DissolveAndDestroy()
    {
        float elapsed = 0f;
        Color startColor = spriteRenderer.color;

        while (elapsed < dissolveTime)
        {
            float t = elapsed / dissolveTime;
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
