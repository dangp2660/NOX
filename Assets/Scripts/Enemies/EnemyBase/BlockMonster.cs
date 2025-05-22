using System.Collections; 
using UnityEngine; 
 
public class BlockMonster : Enemy 
{ 
    [Header("BlockMonster Settings")]
    [SerializeField] private GameObject Boss;
    [SerializeField] private float blockDuration = 5f; 
    [SerializeField] private float blockCooldown = 8f; 
    [SerializeField] private float directionBlockRange = 3f; 
    [SerializeField] private float timeToAttackAfterDetect = 1.5f; 
    [SerializeField] private float blockAngleThreshold = 60f; // Angle threshold for successful blocking
 
    private float blockTimer = 0f; 
    private bool isBlocking = false; 
    private bool attack = false; 
 
    [SerializeField] private DetectionZone zone; 
 
    private Damageable damageable; 
    private Transform player; 
    private EnemyFollow follow; 
 
    public bool IsBlocking 
    { 
        get => isBlocking; 
        set 
        { 
            isBlocking = value; 
            animator.SetBool(AnimationStringList.isBlocking, value); 
        } 
    } 
 
    public new bool Attack 
    { 
        get => attack; 
        set 
        { 
            attack = value; 
            animator.SetBool(AnimationStringList.Attack, value); 
        } 
    } 
 
    private void Awake() 
    { 
        damageable = GetComponent<Damageable>(); 
        animator = GetComponent<Animator>(); 
        zone = GetComponentInChildren<DetectionZone>(); 
        player = GameObject.FindGameObjectWithTag("Player")?.transform; 
        follow = GetComponent<EnemyFollow>(); 
    } 
 
    protected override void Update() 
    { 
        base.Update(); 
 
        if (player == null) return;
        
        // Check if monster is dead
        if (damageable != null && !damageable.IsAlive) return;
        
        // Handle blocking logic
        HandleBlockingLogic();
        
        // If blocking, don't process other behaviors
        if (IsBlocking) return;
        
        // Handle attack detection
        HandleAttackDetection();
        
        // Handle player death
        HandlePlayerDeath();
        
        // Handle movement when patrolling
        HandleMovement();
        
        // Update enemy state
        UpdateEnemyState();
        
        // Handle facing direction
        HandleFacing();
    }
    
    private void HandleBlockingLogic()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Start blocking if conditions are met
        if (blockTimer <= 0 && !IsBlocking && distanceToPlayer <= directionBlockRange)
        {
            StartBlock();
        }
        
        // Update block timer if currently blocking
        if (IsBlocking)
        {
            blockTimer -= Time.deltaTime;
            if (blockTimer <= 0)
            {
                StopBlock();
            }
        }
    }
    
    private void HandleAttackDetection()
    {
        if (zone.detectedColliders.Count > 0 && !Attack)
        {
            StartCoroutine(DelayAttack(timeToAttackAfterDetect));
        }
    }
    
    private void HandlePlayerDeath()
    {
        bool isPlayerAlive = player.GetComponent<PlayerHealth>().isAlive();
        if (!isPlayerAlive)
        {
            Destroy(gameObject);
        }
        if (!Boss.GetComponent<Damageable>().IsAlive)
        {
            Destroy(gameObject);
        }
    }
    
    private void HandleMovement()
    {
        if (enemyState == EnemyState.Patrol && follow != null && !Attack)
        {
            follow.handleMove();
        }
    }
    
    private void UpdateEnemyState()
    {
        Attack = zone.detectedColliders.Count > 0;
        if (Attack)
        {
            switchState(EnemyState.Attack);
        }
        else
        {
            switchState(EnemyState.Patrol);
        }
    }
    
    private IEnumerator DelayAttack(float time) 
    { 
        Attack = true; 
        IsBlocking = false; 
        yield return new WaitForSeconds(time + 0.2f); 
        Attack = false; 
        animator.SetBool(AnimationStringList.Attack, false); 
        IsBlocking = true; 
    } 
 
    private void StartBlock() 
    { 
        IsBlocking = true; 
        blockTimer = blockDuration; 
        
        // Face the player when starting to block
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }
    } 
 
    private void StopBlock() 
    { 
        IsBlocking = false; 
        blockTimer = blockCooldown; 
    } 
    
    // Instead of overriding, create a new method to handle facing
    private void HandleFacing()
    {
        // Only allow flipping if not blocking
        if (!IsBlocking && player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }
    }
    
    // Method to check if damage should be blocked based on attack direction
    public bool AttemptDamage(float damage, Vector2 attackPosition)
    {
        if (!IsBlocking)
        {
            return true; // Not blocking, so damage goes through
        }
        
        // Calculate angle between monster's forward direction and attack direction
        Vector2 monsterPosition = transform.position;
        Vector2 monsterForward = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 attackDirection = (attackPosition - monsterPosition).normalized;
        
        float angle = Vector2.Angle(monsterForward, attackDirection);
        
        // If attack is coming from the front (within threshold angle), block it
        if (angle <= blockAngleThreshold)
        {
            // Play block animation or effect here if needed
            animator.SetTrigger("BlockHit");
            Debug.Log($"{gameObject.name} blocked attack from angle: {angle}");
            return false; // Damage blocked
        }
        
        // Attack came from behind or outside blocking angle
        return true; // Damage goes through
    }
}
