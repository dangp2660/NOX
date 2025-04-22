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
    private float currentHP;
    protected Animator ani;

    [SerializeField] protected EnemyStae enemyStae = EnemyStae.Patrol;
    [SerializeField] private EnemyPatrol patrol;
    void Start()
    {
        
            ani = GetComponent<Animator>();
            patrol = GetComponent<EnemyPatrol>();

            if (DataStat != null)
            {
                currentHP = DataStat.Hp;
            }
            else
            {
                Debug.LogWarning("DataStat chưa được gán! Dùng HP mặc định");
                currentHP = 100; // hoặc bất kỳ giá trị mặc định nào
            }
        

    }

    // Update is called once per frame
    protected void Update()
    {
        switch (enemyStae)
        {
            case EnemyStae.Patrol:
                patrol.enabled = true; break;
            case EnemyStae.Attack:
                
                patrol.enabled = false;
                break;
            case EnemyStae.Die:
                patrol.enabled = false;
                ani.SetTrigger(AnimationStringList.Die);
                StartCoroutine(DelayDestroy());
                break;
        }
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(2f);
        DestroyObject(gameObject);
    }

    protected void swichState(EnemyStae enemyStae)
    {
        if (enemyStae == this.enemyStae) return;
        this.enemyStae = enemyStae;
    }

    public void TakeDame(float Dame)
    {
        Debug.Log(currentHP);
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
        this.currentHP = data.Hp;
    }

}