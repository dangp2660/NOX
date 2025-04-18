using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    [SerializeField] private Data Data;
    private float dame;
    // Start is called before the first frame update
    void Start()
    {
        dame = Data.Dame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (dame != null)
        {
            playerHealth.TakeDamage(dame);
        }
    }
}
