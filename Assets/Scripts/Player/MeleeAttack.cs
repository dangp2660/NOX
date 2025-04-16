using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] protected Transform AttackPoint;
    [SerializeField] protected float AttackRange = 0.5f;
    [SerializeField] protected LayerMask EnemyLayer;


    // Cho phép override từ class con
    public void meleeAttack(float dame)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayer);
        foreach (Collider2D hit in hits)
        {
            Debug.Log("hit");
            hit.GetComponent<Enemy>().TakeDame(dame);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
