using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    [Header("Attack")]
    [SerializeField] private DetectionZone zone;
    private bool attack = false;
    public new bool Attack
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
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() 
    {
        base.Update();
        Attack = zone.detectedColliders.Count > 0;
        if(Attack)
        {
            switchState(EnemyState.Attack);  
        }
        else
        {
            switchState(EnemyState.Patrol);
        }
    }
}