using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{
    // Start is called before the first frame update
    [Header("Attack")]
    [SerializeField] private DetectionZone attackZone;
    [SerializeField] private float coolDown = 2f;
    private MagicAttack range;
    private float coolDownTimer = 0;
    private bool attack = false;
    public bool isAttack
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

        range = GetComponent<MagicAttack>();
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
        isAttack = attackZone.detectedColliders.Count > 0;
        if (isAttack)
        {
            switchState(EnemyState.Attack);
        }
        else
        {
            switchState(EnemyState.Patrol);
        }
    }

    protected override void Attack()
    {
        coolDownTimer = coolDown;
        animator.SetTrigger(AnimationStringList.Attack);
    }
    public void fire()
    {
        range.firePrefabs();
    }
}
