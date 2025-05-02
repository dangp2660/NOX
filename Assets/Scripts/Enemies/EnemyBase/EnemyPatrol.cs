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
    private bool isSwitching = false;

    private bool CanMove => !animator.GetBool(AnimationStringList.Attack);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = PointB.transform;
    }

    void Update()
    {
        if (isMoving && CanMove)
        {
            MovePatrol();
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        animator.SetBool(AnimationStringList.isRunning, isMoving && CanMove);
    }

    private void MovePatrol()
    {
        Vector2 direction = currentPoint.position - transform.position;
        rb.velocity = new Vector2(Mathf.Sign(direction.x) * Speed, rb.velocity.y);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && !isSwitching)
        {
            StartCoroutine(SwitchDirectionAfterDelay(1f));
        }
    }

    private IEnumerator SwitchDirectionAfterDelay(float delay)
    {
        isSwitching = true;
        isMoving = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(delay);

        Flip();
        currentPoint = (currentPoint == PointA.transform) ? PointB.transform : PointA.transform;
        isMoving = true;
        isSwitching = false;
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
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
