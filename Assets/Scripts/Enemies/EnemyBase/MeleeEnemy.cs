using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] private Data DataStats;
    private float currentHP;

    private void Start()
    {
        SetData(DataStats);
        currentHP = DataStats.Hp;
    }
    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDame(float Dame)
    {
        currentHP -= Dame;
        if (currentHP < 0)
        {
            swichState(EnemyStae.Die);
        }
    }

}
