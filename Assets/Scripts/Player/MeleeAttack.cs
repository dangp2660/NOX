using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("Dame")]
    [SerializeField] private Data attackData;
    [SerializeField] private float damageRate;
    private float currentDamage;


    private void Awake()
    {
        currentDamage = attackData.Dame;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        BreakDoor breakDoor = collision.GetComponent<BreakDoor>();
        if(breakDoor != null)
        {
            Debug.Log("Break");
            breakDoor.Break();
        }

        if (damageable != null)
        {
            Debug.Log("Hit");
            damageable.TakeDamage(currentDamage, damageRate);
        }
    }
}
