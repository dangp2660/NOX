using UnityEngine;

public enum EnemyStae
{
    Patrol, Attack, Die, Hurt
}

public abstract class Enemy : MonoBehaviour
{

    [SerializeField] private Data dataStat;
    [SerializeField] protected EnemyStae enemyStae = EnemyStae.Patrol;
    private EnemyPatrol patrol;
    void Start()
    {
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
                break;
            case EnemyStae.Die:
                break;
        }
    }

    protected void swichState(EnemyStae enemyStae)
    {
        if (enemyStae == this.enemyStae) return;
        this.enemyStae = enemyStae;
    }

    protected abstract void Attack();

}
