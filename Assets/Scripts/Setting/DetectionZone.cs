using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent noCollidersRemain;
    [SerializeField] private Damageable Damageable;
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    private Collider2D col;
    private PlayerHealth health;
    private void Awake()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Can Attack");
        PlayerHealth newHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        health = newHealth;
        if (health != null)
        {
            if(!health.isAlive() || !Damageable.IsAlive)
            {
                detectedColliders.Remove(collision);
                Debug.Log("Delete");
                return;
            }
        }
        if (collision.CompareTag("Player"))
        {
            if (!detectedColliders.Contains(collision))
            {
                detectedColliders.Add(collision);
            }
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
    public void ForceRecheck(Collider2D collider)
    {
        if (collider != null && collider.CompareTag("Player"))
        {
            if (!detectedColliders.Contains(collider))
            {
                detectedColliders.Add(collider);
            }
        }
    }

    private void Update()
    {
           
    }
}
