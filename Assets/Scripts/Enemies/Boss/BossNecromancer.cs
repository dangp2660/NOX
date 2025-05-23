using UnityEngine;
using System.Collections;

public enum BossPhase { Phase1, Phase2 }

public class NecromancerBoss : MonoBehaviour
{
    [SerializeField] private GameObject Arena;
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

    [Header("Skill Cooldowns")]
    [SerializeField] private float laserInterval = 20f;
    [SerializeField] private float laserDuration = 3f;
    [SerializeField] private float fireColumnInterval = 10f;
    [SerializeField] private float meteorInterval = 12f;
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
    private Vector3 initialPosition; // Vị trí ban đầu của boss
    [SerializeField] private GameObject CutsceneEnd;
    void Start()
    {
        Arena.SetActive(true);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = false;
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();

        initialPosition = transform.position; // Lưu lại vị trí ban đầu

        StartCoroutine(MovementCycle());

        phaseOneAttackCoroutine = StartCoroutine(PhaseOneAttackRoutine());
    }

    void Update()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        GameObject PlayerManager = GameObject.FindGameObjectWithTag("PlayerManager");
        if (Player != null && !Player.GetComponent<Damageable>().IsAlive)
        {
            // Player chết -> reset boss về vị trí ban đầu
            ResetBossPosition();
        }

        if (!damageable.IsAlive)
        {
            
            UIOpen UI = GameObject.FindGameObjectWithTag("UI").GetComponent<UIOpen>();
            animator.SetBool(AnimationStringList.isAlive, false);
            Player.GetComponent<PlayerController>().DisableSignal();
            PlayerManager.GetComponent<PlayerSwitch>().DisableSignal();
            UI.gameObject.SetActive(false);
            Arena.SetActive(false);
            CutsceneEnd.SetActive(true);
            this.enabled = false;
        }
        else
        {
            FacePlayer();
            CheckPhaseChange();
        }
    }

    private void ResetBossPosition()
    {
        // Dừng tất cả coroutine tấn công, di chuyển
        if (phaseOneAttackCoroutine != null)
        {
            StopCoroutine(phaseOneAttackCoroutine);
            phaseOneAttackCoroutine = null;
        }

        StopAllCoroutines(); // Dừng các coroutine khác như MovementCycle, PhaseTwoSkillRoutine, SummonMinions...

        // Đặt boss về vị trí ban đầu
        transform.position = initialPosition;

        // Reset trạng thái boss
        currentWaypoint = 0;
        isMoving = true;
        currentPhase = BossPhase.Phase1;
        phase2Started = false;

        // Reset máu boss nếu muốn (tuỳ bạn)
        damageable.CurrentHealth = damageable.getMaxHealth();

        // Bật lại coroutine tấn công và di chuyển cho boss
        phaseOneAttackCoroutine = StartCoroutine(PhaseOneAttackRoutine());
        StartCoroutine(MovementCycle());
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
        while (damageable.IsAlive)
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
    private void ShootProjectileAtPlayer(GameObject projectilePrefab, Vector3 spawnPos)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 direction = (player.transform.position - spawnPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Quay projectile đúng hướng
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.Euler(0, 0, angle));
    }

    private void Attack()
    {
        GameObject[] spells = currentPhase == BossPhase.Phase1 ? phaseOneSpells : null;
        if (spells != null && spells.Length > 0)
        {
            int spellIndex = Random.Range(0, spells.Length);
            GameObject selectedSpell = spells[spellIndex];

            animator.SetTrigger("AttackSpell");

            ShootProjectileAtPlayer(selectedSpell, transform.position);
        }
    }


    private IEnumerator PhaseOneAttackRoutine()
    {
        while (damageable.IsAlive)
        {
            yield return new WaitForSeconds(phaseOneAttackInterval);
            Attack();
        }
    }


    private void CheckPhaseChange()
    {
        if (damageable.IsAlive)
        {
            if (!phase2Started && damageable.CurrentHealth <= damageable.getMaxHealth() / 2)
            {
                phase2Started = true;
                currentPhase = BossPhase.Phase2;

                if (phaseOneAttackCoroutine != null)
                    StopCoroutine(phaseOneAttackCoroutine);

                StartCoroutine(PhaseTwoSkillRoutine());
                StartCoroutine(UseMeteor());

            }
        }
    }

    private IEnumerator PhaseTwoSkillRoutine()
    {
        while (damageable.IsAlive)
        {
            yield return StartCoroutine(UseLaser());
            yield return new WaitForSeconds(laserInterval);

            yield return StartCoroutine(UseFireColumn());
            yield return new WaitForSeconds(fireColumnInterval);

            yield return StartCoroutine(ApplyBuff());
            yield return new WaitForSeconds(buffInterval);
        }
    }

    private IEnumerator UseLaser()
    {
        Debug.Log("Use Laser 5 Shots");

        for (int i = 0; i < 5; i++)
        {
            animator.SetTrigger("AttackSpell2");
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            ShootProjectileAtPlayer(laserPrefab, pos);

            yield return new WaitForSeconds(1.25f); // Thời gian giữa mỗi phát
        }
    }



    private IEnumerator UseFireColumn()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Use fire column");
            animator.SetTrigger("AttackSpell");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                Instantiate(fireColumnPrefab, player.transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator UseMeteor()
    {
        while (damageable.IsAlive)
        {
            for (int i = 0; i < 3; i++)
            {
                Debug.Log("Use Meteor");
                animator.SetTrigger("AttackSpell");

                Vector3 randomPos = new Vector3(Random.Range(170, 200), 27, 0);

                // Tạo Quaternion quay hướng xuống (tức là góc 90 hoặc -90 độ tùy hướng sprite)
                Quaternion rotation = Quaternion.Euler(0, 0, -90f); // -90 độ xoay quanh trục Z

                Instantiate(meteorPrefab, randomPos, rotation);
            }

            yield return new WaitForSeconds(meteorInterval);
        }
    }


    

    private IEnumerator ApplyBuff()
    {
        if(damageable.IsAlive)
        {
            animator.SetTrigger("Buff");
            damageable.CurrentHealth += damageable.getMaxHealth() * 0.02f;
            yield return null;
        }
        
    }

    public void OnEnable()
    {
        this.enabled = true;
    }
    public void OnDisable()
    {
        this.enabled = false;
    }
}
