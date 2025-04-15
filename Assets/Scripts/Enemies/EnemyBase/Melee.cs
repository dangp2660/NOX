using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Enemy
{
    [SerializeField] private GameObject AttackPoint;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected override void Attack()
    {
        Debug.Log("attack");
    }
}
