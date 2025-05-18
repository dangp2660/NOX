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
        Damageable playerHealth = collision.GetComponent<Damageable>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(dame,1);
        }
    }
}
