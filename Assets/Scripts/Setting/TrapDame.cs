using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDame : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float dame = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damegeable playerHealth = collision.GetComponent<Damegeable>();
        if (playerHealth != null)
        {
            playerHealth.TakeDame(dame);
        }
    }
}
