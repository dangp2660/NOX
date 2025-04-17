using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    public float Speed = 2f;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform currentPoint;
    private bool isMoving = true;
    private bool isSwitching = false; // NEW: tránh gọi Coroutine nhiều lần

    private bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStringList.Attack);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = PointB.transform;
    }

    void Update()
    {
        if (isMoving)
        {
            MovePatrol();
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        animator.SetBool(AnimationStringList.isRunning, isMoving);
    }

    private void MovePatrol()
    {
        if(!CanMove)
        {
            Vector2 direction = currentPoint.position - transform.position;
            rb.velocity = new Vector2(Mathf.Sign(direction.x) * Speed, rb.velocity.y); // luôn chạy với tốc độ cố định

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && !isSwitching)
            {
                StartCoroutine(SwitchDirectionAfterDelay(1f));
            }
        }
        else 
            rb.velocity = Vector2.zero;
        
    }

    private IEnumerator SwitchDirectionAfterDelay(float delay)
    {
        isSwitching = true;
        isMoving = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(delay);
        flip();
        currentPoint = (currentPoint == PointA.transform) ? PointB.transform : PointA.transform;
        isMoving = true;
        isSwitching = false;
    }

    private void flip()
    {
        Vector3 a = transform.position;
       
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        transform .localPosition = a;
    }

    private void OnDrawGizmos()
    {
        if (PointA != null && PointB != null)
        {
            Gizmos.DrawWireSphere(PointA.transform.position, 0.5f);
            Gizmos.DrawWireSphere(PointB.transform.position, 0.5f);
            Gizmos.DrawLine(PointA.transform.position, PointB.transform.position);
        }
    }
}
