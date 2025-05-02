using UnityEngine;

public class EnemyRayDetector : MonoBehaviour
{
    public float detectionRange = 5f;
    public LayerMask playerLayer;
    public Transform rayOrigin; // thường là transform của enemy

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, Vector2.right * transform.localScale.x, detectionRange, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log("Player detected by raycast");
            // Trigger wakeup or switch state
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (rayOrigin == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin.position, rayOrigin.position + Vector3.right * transform.localScale.x * detectionRange);
    }
}
