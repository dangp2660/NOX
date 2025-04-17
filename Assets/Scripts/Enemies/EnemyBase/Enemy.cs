using UnityEngine;

public enum EnemyStae
{
    Patrol, Attack, Die, Hurt
}

public abstract class Enemy : MonoBehaviour
{
    private Data DataStat;
    private float currentHP;
    
    [SerializeField] protected EnemyStae enemyStae = EnemyStae.Patrol;
    [SerializeField] private EnemyPatrol patrol;
    void Start()
    {
        patrol = GetComponent<EnemyPatrol>();
        currentHP = DataStat.Hp;
    }

    // Update is called once per frame
    protected  void Update()
    {
        switch (enemyStae)
        {
            case EnemyStae.Patrol:
                patrol.enabled = true; break;
            case EnemyStae.Attack:
                Debug.Log("State Attack");
                patrol.enabled = false;
                break;
            case EnemyStae.Die:
                Debug.Log("Die");
                break;
        }
    }

    protected void swichState(EnemyStae enemyStae)
    {
        if (enemyStae == this.enemyStae) return;
        this.enemyStae = enemyStae;
    }

    public  void TakeDame(float Dame)
    {
        currentHP -= Dame;
        if (currentHP <= 0)
        {
            swichState(EnemyStae.Die);
        }

    }
    protected virtual void Attack() { }
    public void SetData(Data data)
    {
        this.DataStat = data;
    }

}
