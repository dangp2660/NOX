using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyStae
{
    Patrol, Attack, Die, Hurt
}

public abstract class Enemy : MonoBehaviour
{
    private Data DataStat;
    protected Animator animator;

    [SerializeField] protected EnemyStae enemyStae = EnemyStae.Patrol;
    [SerializeField] protected EnemyPatrol patrol;
    void Start()
    {

            animator = GetComponent<Animator>();
            patrol = GetComponent<EnemyPatrol>();   

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch (enemyStae)
        {
            case EnemyStae.Patrol:
                patrol.enabled = true; break;
            case EnemyStae.Attack:
                patrol.enabled = false;
                Attack();
                break;
            case EnemyStae.Die:
                patrol.enabled = false;
                animator.SetBool(AnimationStringList.isAlive, false);
                Destroy(gameObject, 2f);
                break;
        }
    }

    public void swichState(EnemyStae enemyStae)
    {
        if (enemyStae == this.enemyStae) return;
        this.enemyStae = enemyStae;
    }
    protected virtual void Attack() { }
    public void SetData(Data data)
    {
        this.DataStat = data;
    }


}