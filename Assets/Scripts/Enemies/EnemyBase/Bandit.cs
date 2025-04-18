using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    [SerializeField] Data Stats;
    [Header("Attack")]
    [SerializeField] private Transform AttackPoint;
    [SerializeField] protected LayerMask EnemyLayer;
    [SerializeField] private float attackRange = 0.5f;

    

    [SerializeField] private DetectionZone zone;
    private bool attack = false;    
    private bool Attack
    {
        get
        {
            return attack;
        }
        set
        {
            attack = value;
            ani.SetBool(AnimationStringList.Attack, value);
        }
    }

    private void Awake()
    {

        ani = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetData(Stats);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        Attack = zone.detectedCollider.Count > 0;
        
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }


}
