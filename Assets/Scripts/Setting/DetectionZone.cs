using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedCollider = new List<Collider2D>();
    private Dictionary<Collider2D, Coroutine> removeTimers = new Dictionary<Collider2D, Coroutine>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!detectedCollider.Contains(collision))
        {
            detectedCollider.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Nếu coroutine đang chạy -> dừng lại
        if (removeTimers.ContainsKey(collision))
        {
            StopCoroutine(removeTimers[collision]);
            removeTimers.Remove(collision);
        }

        // Chạy coroutine từ CoroutineRunner (luôn hoạt động)
        Coroutine removeCoroutine = CoroutineRunner.instance.StartCoroutine(RemoveAfterDelay(collision));
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

        if (detectedCollider.Contains(collider))
        {
            detectedCollider.Remove(collider);
        }

        if (removeTimers.ContainsKey(collider))
        {
            removeTimers.Remove(collider);
        }
    }
}
