using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Enemy
{
    private Damageable Enemy;
    private void Awake()
    {
       Enemy = gameObject.GetComponent<Damageable>();
    }
    void Update()
    {
        if(!Enemy.IsAlive)
        {
            swichState(EnemyStae.Die);
        }
    }
}
