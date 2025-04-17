using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    [SerializeField] Data Stats;
    private Animator animator;
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
            animator.SetBool(AnimationStringList.Attack, value);
        }
    }

    private void Awake()
    {

        animator = GetComponent<Animator>();
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
        if (Attack)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, EnemyLayer);
            foreach (Collider2D player in hit)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(Stats.Dame);
            }
        }
    }
  
   
}
