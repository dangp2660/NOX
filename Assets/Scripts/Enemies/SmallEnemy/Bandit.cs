using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    [SerializeField] Data Stats;
    [Header("Attack")]
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
        Attack = zone.detectedColliders.Count > 0;

    }
}