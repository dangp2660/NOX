using UnityEngine;
using System.Collections;

public enum BossPhase { Phase1, Phase2 }

public class NecromancerBoss : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform summonPos;

    [Header("Phase 1 Spells")]
    [SerializeField] private GameObject[] phaseOneSpells;
    [SerializeField] private float phaseOneAttackInterval = 2.5f;

    [Header("Phase 2 Skills")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject fireColumnPrefab;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private GameObject[] summonMinionsPrefabs;

    [Header("Skill Cooldowns")]
    [SerializeField] private float laserInterval = 20f;
    [SerializeField] private float laserDuration = 3f;
    [SerializeField] private float fireColumnInterval = 10f;
    [SerializeField] private float meteorInterval = 12f;
    [SerializeField] private float summonInterval = 15f;
    [SerializeField] private float buffInterval = 15f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stayDuration = 10f;

    private int currentWaypoint = 0;
    private bool isMoving = true;
    private BossPhase currentPhase = BossPhase.Phase1;
    private bool phase2Started = false;

    private Coroutine phaseOneAttackCoroutine;
    private Damageable damageable;
    private Animator animator;

    void Start()
    {
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();
        StartCoroutine(MovementCycle());

        // Phase 1 attack
        phaseOneAttackCoroutine = StartCoroutine(PhaseOneAttackRoutine());
    }

    void Update()
    {
        if(!damageable.IsAlive) return;
        FacePlayer();
        CheckPhaseChange();
    }

    private void FacePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }
    }

    private IEnumerator MovementCycle()
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
                if (currentPhase == BossPhase.Phase1)
                {
                    Attack();
                }

                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                isMoving = true;
            }
            yield return null;
        }
    }

    private void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypoint];
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            isMoving = false;
        }
    }

    private void Attack()
    {
        GameObject[] spells = currentPhase == BossPhase.Phase1 ? phaseOneSpells : null;
        if (spells != null && spells.Length > 0)
        {
            int spellIndex = Random.Range(0, spells.Length);
            GameObject spell = Instantiate(spells[spellIndex], transform.position, Quaternion.identity);

            animator.SetTrigger("AttackSpell");

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector2 dir = (player.transform.position - transform.position).normalized;
                Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = dir * 5f;
                }
            }
        }
    }

    private IEnumerator PhaseOneAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(phaseOneAttackInterval);
            Attack();
        }
    }

    private void CheckPhaseChange()
    {
        if (!phase2Started && damageable.CurrentHealth <= damageable.getMaxHealth() / 2)
        {
            phase2Started = true;
            currentPhase = BossPhase.Phase2;

            if (phaseOneAttackCoroutine != null)
                StopCoroutine(phaseOneAttackCoroutine);

            StartCoroutine(PhaseTwoSkillRoutine());
        }
    }

    private IEnumerator PhaseTwoSkillRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(SummonMinions());
            yield return new WaitForSeconds(summonInterval);

            yield return StartCoroutine(UseLaser());
            yield return new WaitForSeconds(laserInterval);

            yield return StartCoroutine(UseFireColumn());
            yield return new WaitForSeconds(fireColumnInterval);

            yield return StartCoroutine(UseMeteor());
            yield return new WaitForSeconds(meteorInterval);

            yield return StartCoroutine(ApplyBuff());
            yield return new WaitForSeconds(buffInterval);
        }
    }

    private IEnumerator UseLaser()
    {
        animator.SetTrigger("AttackSpell2");
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        laser.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        Destroy(laser, laserDuration);
        yield return null;
    }

    private IEnumerator UseFireColumn()
    {
        animator.SetTrigger("AttackSpell");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            Instantiate(fireColumnPrefab, player.transform.position, Quaternion.identity);
        }
        yield return null;
    }

    private IEnumerator UseMeteor()
    {
        animator.SetTrigger("AttackSpell");
        Vector3 randomPos = new Vector3(Random.Range(-5, 5), 10, 0);
        Instantiate(meteorPrefab, randomPos, Quaternion.identity);
        yield return null;
    }

    private IEnumerator SummonMinions()
    {
        animator.SetTrigger("AttackSpell2");
        foreach (var minion in summonMinionsPrefabs)
        {
            Instantiate(minion, summonPos.position, Quaternion.identity);
        }
        yield return null;
    }

    private IEnumerator ApplyBuff()
    {
        animator.SetTrigger("Buff");
        damageable.CurrentHealth += damageable.getMaxHealth() * 0.02f;
        yield return null;
    }
}
