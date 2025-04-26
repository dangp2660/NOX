using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent noCollidersRemain;

    public List<Collider2D> detectedColliders = new List<Collider2D>();
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!detectedColliders.Contains(collision))
        {
            detectedColliders.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(RemoveAfterDelay(collision, 0.7f));
    }

    private IEnumerator RemoveAfterDelay(Collider2D collision, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Chỉ remove nếu collider vẫn còn trong danh sách
        if (detectedColliders.Contains(collision))
        {
            detectedColliders.Remove(collision);

            if (detectedColliders.Count <= 0)
            {
                noCollidersRemain.Invoke();
            }
        }
    }

    // Hàm remove thủ công bên ngoài có thể gọi
    public void RemoveCollider(Collider2D collider)
    {
        if (detectedColliders.Contains(collider))
        {
            detectedColliders.Remove(collider);

            if (detectedColliders.Count <= 0)
            {
                noCollidersRemain.Invoke();
            }
        }
    }
    private void Update()
    {
        for (int i = detectedColliders.Count - 1; i >= 0; i--)
        {
            var collider = detectedColliders[i];
            PlayerHealth health = collider.GetComponent<PlayerHealth>();
            if (!health.isAlive())
            {
                detectedColliders.RemoveAt(i);

                if (detectedColliders.Count <= 0)
                {
                    noCollidersRemain.Invoke();
                }
            }
        }
    }
}
