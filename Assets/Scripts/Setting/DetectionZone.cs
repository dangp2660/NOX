using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedCollider = new List<Collider2D>();
    private void Awake()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
         detectedCollider.Add(collision);  
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedCollider.Remove(collision);
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
}
