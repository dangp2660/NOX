using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent noCollidersRemain;
    [SerializeField] private Damageable Damageable;
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    private PlayerHealth health;

    private void Awake()
    {
        // Tìm PlayerHealth một lần duy nhất
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            health = playerObject.GetComponent<PlayerHealth>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Kiểm tra trực tiếp xem Player còn sống không
            if (health != null && (!health.isAlive() || !Damageable.IsAlive))
            {
                RemoveCollider(collision);
                return;
            }

            if (!detectedColliders.Contains(collision))
            {
                detectedColliders.Add(collision);
                Debug.Log("Player entered detection zone.");
            }
        }else if(collision.CompareTag("Door"))
        {
            detectedColliders.Clear();
            noCollidersRemain.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            RemoveCollider(collision);
        }
    }

    // Hàm xóa collider khỏi danh sách
    private void RemoveCollider(Collider2D collider)
    {
        if (detectedColliders.Contains(collider))
        {
            detectedColliders.Remove(collider);
            Debug.Log("Player exited detection zone.");

            if (detectedColliders.Count == 0)
            {
                noCollidersRemain.Invoke();
            }
        }
    }

    // Hàm để thêm collider từ bên ngoài
    public void ForceRecheck(Collider2D collider)
    {
        if (collider != null && collider.CompareTag("Player"))
        {
            if (!detectedColliders.Contains(collider))
            {
                detectedColliders.Add(collider);
                Debug.Log("Player force-rechecked into detection zone.");
            }
        }
    }
}
