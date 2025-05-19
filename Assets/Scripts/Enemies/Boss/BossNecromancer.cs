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
    public float moveSpeed = 3f;
    public float stayDuration = 10f;
    public float summonInterval = 15f;
    public float buffInterval = 15f;
    public float fireColumnInterval = 10f;
    public float meteorInterval = 12f;
    public float laserInterval = 20f;
    public float laserDuration = 3f;
    private int currentWaypoint = 0;
    private bool isMoving = true;
    private BossPhase currentPhase = BossPhase.Phase1;
    private Damageable damageable;

    void Start()
    {
        damageable = GetComponent<Damageable>();
        StartCoroutine(MovementCycle());
        StartCoroutine(FireColumnRoutine());
        StartCoroutine(MeteorRoutine());
        StartCoroutine(LaserRoutine());
        StartCoroutine(SummonRoutine());
        StartCoroutine(BuffRoutine());
    }

    void Update()
    {
        FacePlayer();
        CheckPhaseChange();
    }

    public void FacePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }
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
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                isMoving = true;
            }
            yield return null;
        }
    }

    public void MoveToWaypoint()
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
        GameObject[] spells = currentPhase == BossPhase.Phase1 ? phaseOneSpells : null;
        if (spells.Length > 0)
        {
            int spellIndex = Random.Range(0, spells.Length);
            Instantiate(spells[spellIndex], transform.position, Quaternion.identity);
        }
    }

    public void CheckPhaseChange()
    {
        if (damageable.CurrentHealth <= damageable.getMaxHealth() / 2)
        {
            currentPhase = BossPhase.Phase2;
        }
    }

    IEnumerator LaserRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(laserInterval);
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            Destroy(laser, laserDuration);
        }
    }

    IEnumerator FireColumnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireColumnInterval);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                Instantiate(fireColumnPrefab, player.transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator MeteorRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(meteorInterval);
            Vector3 randomPos = new Vector3(Random.Range(-5, 5), 10, 0);
            Instantiate(meteorPrefab, randomPos, Quaternion.identity);
        }
    }

    IEnumerator SummonRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(summonInterval);
            foreach (var minion in summonMinionsPrefabs)
            {
                Instantiate(minion, transform.position + Vector3.right, Quaternion.identity);
            }
        }
    }

    IEnumerator BuffRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(buffInterval);
            damageable.CurrentHealth += damageable.getMaxHealth() * 0.02f;
        }
    }
}
