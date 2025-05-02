using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Enemy baseEnemy;
    private Damageable Enemy;
    private void Awake()
    {
       baseEnemy = GetComponent<Enemy>();
       Enemy = gameObject.GetComponent<Damageable>();
    }
    void Update()
    {
        if(!Enemy.IsAlive)
        {
            baseEnemy.switchState(EnemyState.Die);
        }
    }
}
