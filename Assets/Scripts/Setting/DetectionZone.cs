using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedCollider = new List<Collider2D>();
    private Dictionary<Collider2D, Coroutine> removeTimers = new Dictionary<Collider2D, Coroutine>(); // To store coroutines for each collider

    private void Awake()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedCollider.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If there is already a coroutine running for this collider, stop it.
        if (removeTimers.ContainsKey(collision))
        {
            StopCoroutine(removeTimers[collision]);
            removeTimers.Remove(collision);
        }

        // Start a new coroutine to remove it after 1.2 seconds
        Coroutine removeCoroutine = StartCoroutine(RemoveAfterDelay(collision));
        removeTimers.Add(collision, removeCoroutine);
    }
    private void Update()
    {
        for (int i = detectedCollider.Count - 1; i >= 0; i--)
        {
            var health = detectedCollider[i].GetComponent<PlayerHealth>();
            if (health != null && !health.IsAlive())
            {
                detectedCollider.RemoveAt(i);
            }
        }
    }

    private IEnumerator RemoveAfterDelay(Collider2D collider)
    {
        yield return new WaitForSeconds(0.7f);

        // Only remove it if it has not already been removed
        if (detectedCollider.Contains(collider))
        {
            detectedCollider.Remove(collider);
        }
    }
}
