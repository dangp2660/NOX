using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState
{
    Patrol, Attack, Die, Hurt, Sleep, Wakeup
}

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private string checkpointID; 
    private Vector3 spawnPoint;
    protected EnemyState enemyState;
    [SerializeField] protected EnemyState initialState = EnemyState.Patrol;

    protected Animator animator;
    protected EnemyPatrol patrol;

    protected virtual void Start()
    {
        EnemyManager.instance.addEnemy(this);
        spawnPoint = transform.position;
        animator = GetComponent<Animator>();
        patrol = GetComponent<EnemyPatrol>();
        enemyState = initialState; // Gán trạng thái ban đầu từ prefab
    }

    public void respawnEnemy()
    {
        gameObject.SetActive(true);
        transform.position = spawnPoint;
        enemyState = initialState;
        if (patrol != null) { patrol.enabled = true; }
        animator.SetBool(AnimationStringList.isAlive, true);
    }

    public string getCheckPointID()
    {
        return checkpointID;
    }

    protected virtual void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Patrol:
                if (patrol != null) patrol.enabled = true;
                break;

            case EnemyState.Attack:
                if (patrol != null) patrol.enabled = false;
                Attack();
                break;

            case EnemyState.Die:
                if (patrol != null) patrol.enabled = false;
                Die();
                break;

            case EnemyState.Hurt:
                if (patrol != null) patrol.enabled = false;
                Hurt();
                break;
        }
    }

    public void switchState(EnemyState newState)
    {
        if (newState == enemyState) return;
        enemyState = newState;
    }

    protected virtual void Attack() { }
    protected virtual void Hurt() { }
    protected virtual void Die()
    {
        animator.SetBool(AnimationStringList.isAlive, false);
        Destroy(gameObject, 2f);
    }

    public void SetData(Data data) { }
}
