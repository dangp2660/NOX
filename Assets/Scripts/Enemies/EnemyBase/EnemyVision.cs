using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private float visionRange = 5f;
    [SerializeField] LayerMask detectionMask = LayerMask.GetMask("Player", "Obstacle");

    public bool canSeePlayer(Transform Player)
    {
        Vector2 direction = Player.position - transform.position;
        if (direction.sqrMagnitude > visionRange) return false;
        RaycastHit2D hit =  Physics2D.Raycast(transform.position, direction.normalized, visionRange, detectionMask);
        return hit.collider != null && hit.collider.CompareTag("Player");

    }
}
