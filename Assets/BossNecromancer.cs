using UnityEngine;
using System.Collections;

public enum BossPhase { Phase1, Phase2 }

public class BossMovementAndAttack : MonoBehaviour
{
    public Transform[] waypoints;
    public GameObject[] phaseOneSpells;
    public GameObject[] summonMinionsPrefabs;
    public GameObject fireColumnPrefab;
    public GameObject meteorPrefab;
    public GameObject laserPrefab;
    public Transform SummonPosition;

    public float moveSpeed = 3f;
    public float stayDuration = 10f;
    public float fireColumnInterval = 10f;
    public float meteorInterval = 12f;
    public float laserInterval = 20f;
    public float laserDuration = 3f;
    private int currentWaypoint = 0;
    private bool isMoving = true;
    private BossPhase currentPhase = BossPhase.Phase1;
    private Damageable damageable;

    private Coroutine summonCoroutine;
    private Coroutine buffCoroutine;
    private Coroutine fireColumnCoroutine;
    private Coroutine meteorCoroutine;
    private Coroutine laserCoroutine;

    void Start()
    {
        damageable = GetComponent<Damageable>();
        if (damageable == null)
        {
            Debug.LogError("Damageable component not found on boss!");
            return;
        }
        damageable.ResetHealth();
        StartCoroutine(MovementCycle());
    }

    IEnumerator MovementCycle()
    {
        while (true)
        {
            if (isMoving)
            {
                MoveToWaypoint();
            }
            else
            {
                yield return new WaitForSeconds(stayDuration);
                Attack();
                CheckPhaseChange();
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                isMoving = true;
            }
            yield return null;
        }
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypoint];
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            isMoving = false;
        }
    }

    void Attack()
    {
        if (currentPhase == BossPhase.Phase1)
        {
            if (phaseOneSpells.Length > 0)
            {
                int spellIndex = Random.Range(0, phaseOneSpells.Length);
                Instantiate(phaseOneSpells[spellIndex], transform.position, Quaternion.identity);
            }
        }
    }

    void CheckPhaseChange()
    {
        if (currentPhase == BossPhase.Phase1 && damageable.CurrentHealth <= damageable.getMaxHealth() / 2)
        {
            currentPhase = BossPhase.Phase2;
            summonCoroutine = StartCoroutine(SummonRoutine());
            buffCoroutine = StartCoroutine(BuffRoutine());
            fireColumnCoroutine = StartCoroutine(FireColumnRoutine());
            meteorCoroutine = StartCoroutine(MeteorRoutine());
            laserCoroutine = StartCoroutine(LaserRoutine());
        }
    }

    IEnumerator SummonRoutine()
    {
        while (currentPhase == BossPhase.Phase2)
        {
            for (int i = 0; i < 2; i++)
            {
                if (summonMinionsPrefabs.Length > 0)
                {
                    int summonIndex = Random.Range(0, summonMinionsPrefabs.Length);
                    Instantiate(summonMinionsPrefabs[summonIndex], SummonPosition.position, Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(15f);
        }
    }

    IEnumerator BuffRoutine()
    {
        while (currentPhase == BossPhase.Phase2)
        {
            float healAmount = damageable.getMaxHealth() * 0.02f;
            damageable.CurrentHealth = Mathf.Min(damageable.CurrentHealth + healAmount, damageable.getMaxHealth());
            Debug.Log($"Boss healed by {healAmount} HP");
            yield return new WaitForSeconds(15f);
        }
    }

    IEnumerator FireColumnRoutine()
    {
        while (currentPhase == BossPhase.Phase2)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Instantiate(fireColumnPrefab, player.transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(fireColumnInterval);
        }
    }

    IEnumerator MeteorRoutine()
    {
        while (currentPhase == BossPhase.Phase2)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(0f, 5f), 0);
            Instantiate(meteorPrefab, randomPosition, Quaternion.identity);
            yield return new WaitForSeconds(meteorInterval);
        }
    }

    IEnumerator LaserRoutine()
    {
        while (currentPhase == BossPhase.Phase2)
        {
            Vector3 laserPosition = transform.position + Vector3.right * 2f;
            GameObject laser = Instantiate(laserPrefab, laserPosition, Quaternion.identity);
            yield return new WaitForSeconds(laserDuration);
            Destroy(laser);
            yield return new WaitForSeconds(laserInterval);
        }
    }
}
