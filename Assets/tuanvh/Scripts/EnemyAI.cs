using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    [Header("Character References")]
    public Character player;
    public Character enemy;
    
    [Header("AI Behavior Settings")]
    [Range(0f, 1f)] public float aggressiveness = 0.7f; // Tỉ lệ attack
    [Range(0f, 1f)] public float defensiveness = 0.5f;  // Tỉ lệ dodge
    [Range(0.5f, 3f)] public float reactionTime = 1f;   // Thời gian phản ứng
    [Range(0.1f, 2f)] public float decisionCooldown = 0.5f; // Thời gian nghỉ giữa các quyết định
    
    [Header("Debug")]
    [SerializeField] private bool enableDebugLogs = true;
    [SerializeField] private bool enableManualTesting = true;
    
    [SerializeField] private List<AnimationClip> animationClips = new List<AnimationClip>();
    [SerializeField] private float actionTimer = 0f;
    [SerializeField] private float actionDuration = 1f;
    [SerializeField] private float lastDecisionTime = 0f;
    
    // States
    private IdleState idleState;
    private AttackState attackState;
    private DodgeState dodgeState;
    private HitState hitState;
    
    // AI State
    private bool isInvincible = false;
    private bool isPlayerAttacking = false;
    private bool canMakeDecision = true;
    private Coroutine invincibilityCoroutine;
    private Coroutine aiDecisionCoroutine;
    
    // Player monitoring
    private StateMachine playerStateMachine;
    private float playerAttackStartTime;

    private void Awake()
    {
        playerStateMachine = player?.StateMachine;
    }
    
    private void Start()
    {
        InitializeAnimationClip();
        InitializeState();
        InitializePlayerMonitoring();
        StartAIDecisionLoop();
    }

    private void Update()
    {
        HandleActionTimer();
        MonitorPlayerState();
        
        // Manual testing
        if (enableManualTesting)
        {
            HandleManualTesting();
        }
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
        StopAllCoroutines();
    }

    #region Initialization
    private void InitializeAnimationClip()
    {
        if (enemy?.StateMachine?.Animator == null)
        {
            LogDebug("Enemy or Animator not found!");
            return;
        }
        
        Animator animator = enemy.StateMachine.Animator;
        var animatorController = animator.runtimeAnimatorController;
        
        if (animatorController == null)
        {
            LogDebug("Animator Controller not found!");
            return;
        }
        
        AnimationClip[] clips = animatorController.animationClips;
        
        if (clips != null && clips.Length > 0)
        {
            animationClips.Clear();
            animationClips.AddRange(clips);
            LogDebug($"Loaded {clips.Length} animation clips");
        }
    }

    private void InitializeState()
    {
        idleState = new IdleState();
        attackState = new AttackState();
        dodgeState = new DodgeState();
        hitState = new HitState();

        SubscribeToEvents();
        enemy.StateMachine.ChangeState(idleState);
    }

    private void InitializePlayerMonitoring()
    {
        if (player?.StateMachine != null)
        {
            playerStateMachine = player.StateMachine;
        }
    }

    private void SubscribeToEvents()
    {
        if (enemy != null)
            enemy.OnGetHit += ChangeToHitState;
            
        if (idleState != null)
            idleState.OnStateEnter += OnIdleStateEnter;
            
        if (attackState != null)
        {
            attackState.OnStateEnter += OnAttackStateEnter;
            attackState.OnStateExit += OnAttackStateExit;
        }
        
        if (dodgeState != null)
        {
            dodgeState.OnStateEnter += OnDodgeStateEnter;
            dodgeState.OnStateExit += OnDodgeStateExit;
        }
        
        if (hitState != null)
        {
            hitState.OnStateEnter += OnHitStateEnter;
            hitState.OnStateExit += OnHitStateExit;
        }
    }

    private void UnsubscribeFromEvents()
    {
        if (enemy != null)
            enemy.OnGetHit -= ChangeToHitState;
            
        if (idleState != null)
            idleState.OnStateEnter -= OnIdleStateEnter;
            
        if (attackState != null)
        {
            attackState.OnStateEnter -= OnAttackStateEnter;
            attackState.OnStateExit -= OnAttackStateExit;
        }
        
        if (dodgeState != null)
        {
            dodgeState.OnStateEnter -= OnDodgeStateEnter;
            dodgeState.OnStateExit -= OnDodgeStateExit;
        }
        
        if (hitState != null)
        {
            hitState.OnStateEnter -= OnHitStateEnter;
            hitState.OnStateExit -= OnHitStateExit;
        }
    }
    #endregion

    #region AI Decision Making
    private void StartAIDecisionLoop()
    {
        if (aiDecisionCoroutine != null)
            StopCoroutine(aiDecisionCoroutine);
            
        aiDecisionCoroutine = StartCoroutine(AIDecisionCoroutine());
    }

    private IEnumerator AIDecisionCoroutine()
    {
        while (enabled && gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(decisionCooldown);
            
            if (canMakeDecision && enemy.StateMachine.CurrentState is IdleState)
            {
                MakeAIDecision();
            }
        }
    }

    private void MakeAIDecision()
    {
        if (player == null || enemy == null) return;
        
        // Kiểm tra khoảng cách đến player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        // Nếu player đang tấn công và trong tầm
        if (isPlayerAttacking && distanceToPlayer <= GetAttackRange())
        {
            // Quyết định dodge hay không
            if (ShouldDodge())
            {
                PerformDodge();
                return;
            }
        }
        
        // Nếu player trong tầm tấn công
        if (distanceToPlayer <= GetAttackRange())
        {
            // Quyết định tấn công
            if (ShouldAttack())
            {
                PerformAttack();
                return;
            }
        }
        
        // Nếu không có hành động nào, ở trạng thái idle
        lastDecisionTime = Time.time;
    }

    private bool ShouldAttack()
    {
        // Tính toán xác suất tấn công dựa trên aggressiveness
        float attackChance = aggressiveness;
        
        // Tăng xác suất nếu player đang idle
        if (playerStateMachine?.CurrentState is IdleState)
        {
            attackChance += 0.2f;
        }
        
        // Giảm xác suất nếu player đang tấn công
        if (isPlayerAttacking)
        {
            attackChance -= 0.3f;
        }
        
        return Random.Range(0f, 1f) < attackChance;
    }

    private bool ShouldDodge()
    {
        // Tính toán xác suất dodge dựa trên defensiveness
        float dodgeChance = defensiveness;
        
        // Tăng xác suất dodge nếu player đang tấn công
        if (isPlayerAttacking)
        {
            dodgeChance += 0.3f;
        }
        
        // Tính thời gian phản ứng
        float timeSincePlayerAttack = Time.time - playerAttackStartTime;
        if (timeSincePlayerAttack > reactionTime)
        {
            dodgeChance *= 0.5f; // Giảm khả năng dodge nếu phản ứng chậm
        }
        
        return Random.Range(0f, 1f) < dodgeChance;
    }

    private void PerformAttack()
    {
        int attackID = Random.Range(0, 4); // 4 loại tấn công
        ChangeToAttackState(attackID);
    }

    private void PerformDodge()
    {
        // Chọn hướng dodge dựa trên vị trí player
        int dodgeID = DetermineDodgeDirection();
        ChangeToDodgeState(dodgeID);
    }

    private int DetermineDodgeDirection()
    {
        if (player == null) return Random.Range(0, 2);
        
        // Tính toán hướng dodge dựa trên vị trí tương đối
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        
        // Nếu player ở bên trái, dodge sang phải (ID = 1)
        // Nếu player ở bên phải, dodge sang trái (ID = 0)
        return directionToPlayer.x > 0 ? 1 : 0;
    }

    private float GetAttackRange()
    {
        // Có thể điều chỉnh dựa trên loại enemy
        return 2f;
    }
    #endregion

    #region Player Monitoring
    private void MonitorPlayerState()
    {
        if (playerStateMachine == null) return;
        
        bool wasPlayerAttacking = isPlayerAttacking;
        isPlayerAttacking = playerStateMachine.CurrentState is AttackState;
        
        // Ghi nhận thời điểm player bắt đầu tấn công
        if (!wasPlayerAttacking && isPlayerAttacking)
        {
            playerAttackStartTime = Time.time;
            LogDebug("Player started attacking!");
        }
    }
    #endregion

    #region Timer and Animation Management
    private void HandleActionTimer()
    {
        actionDuration = enemy.StateMachine.CurrentState switch
        {
            AttackState => GetLengthOfAttackClip(attackState.AttackID) / enemy.Data.agility,
            DodgeState => GetLengthOfDodgeClip(dodgeState.DodgeID) / enemy.Data.agility,
            HitState => GetLengthOfHitClip(hitState.HitID),
            IdleState => GetLengthOfIdleClip(),
            _ => actionDuration
        };
        
        actionTimer += Time.deltaTime;
        
        // Chỉ thay đổi state khi action hoàn thành
        if (actionTimer >= actionDuration)
        {
            if (enemy.StateMachine.CurrentState is not IdleState)
            {
                ChangeToIdleState();
            }
        }
    }

    private float GetLengthOfAttackClip(int id)
    {
        string clipName = id switch
        {
            0 => "Head Punch",
            1 => "Stomach Punch",
            2 => "Kidney Punch Left",
            3 => "Kidney Punch Right",
            _ => "Head Punch"
        };
        
        return GetAnimationLength(clipName);
    }

    private float GetLengthOfDodgeClip(int id)
    {
        string clipName = id switch
        {
            0 => "Dodge L",
            1 => "Dodge R",
            _ => "Dodge L"
        };
        
        return GetAnimationLength(clipName);
    }

    private float GetLengthOfHitClip(int id)
    {
        string clipName = id switch
        {
            0 => "Head Hit",
            1 => "Stomach Hit",
            2 or 3 => "Kidney Hit L",
            _ => "Head Hit"
        };
        
        return GetAnimationLength(clipName);
    }

    private float GetLengthOfIdleClip()
    {
        return GetAnimationLength("Idle");
    }

    private float GetAnimationLength(string clipName)
    {
        var clip = animationClips.FirstOrDefault(obj => obj.name == clipName);
        return clip != null ? clip.length : 1f; // Default duration if clip not found
    }
    #endregion

    #region State Changes
    private void ChangeToIdleState()
    {
        if (enemy.StateMachine.CurrentState is IdleState) return;
        
        enemy.StateMachine.ChangeState(idleState);
        actionTimer = 0f;
        canMakeDecision = true;
        LogDebug("Changed to Idle State");
    }
    
    private void ChangeToAttackState(int id)
    {
        attackState.AttackID = id;
        attackState.Speed = enemy.Data.agility;
        enemy.StateMachine.ChangeState(attackState);
        actionTimer = 0f;
        canMakeDecision = false;
        LogDebug($"Changed to Attack State (ID: {id})");
    }

    private void ChangeToDodgeState(int id)
    {
        dodgeState.DodgeID = id;
        enemy.StateMachine.ChangeState(dodgeState);
        actionTimer = 0f;
        canMakeDecision = false;
        
        // Bắt đầu invincibility frames
        if (invincibilityCoroutine != null)
            StopCoroutine(invincibilityCoroutine);
            
        invincibilityCoroutine = StartCoroutine(ManageInvincibilityFrames());
        LogDebug($"Changed to Dodge State (ID: {id})");
    }

    private IEnumerator ManageInvincibilityFrames()
    {
        isInvincible = true;
        LogDebug("Invincibility started");
        
        // Chờ một chút để đảm bảo dodge animation bắt đầu
        yield return new WaitForSeconds(0.1f);
        
        // Giữ invincible trong 80% thời gian dodge
        float invincibleDuration = actionDuration * 0.8f;
        yield return new WaitForSeconds(invincibleDuration);
        
        isInvincible = false;
        LogDebug("Invincibility ended");
    }

    private void ChangeToHitState(int id, int damage)
    {
        LogDebug($"ChangeToHitState called - Invincible: {isInvincible}, Damage: {damage}");
        
        // Kiểm tra invincibility
        if (isInvincible)
        {
            LogDebug("Hit blocked - Enemy is invincible!");
            return;
        }
        
        // Dừng tất cả coroutines
        StopAllCoroutines();
        isInvincible = false;
        canMakeDecision = false;
        
        // Xử lý state transition
        var currentState = enemy.StateMachine.CurrentState;
        if (currentState is AttackState)
        {
            OnAttackStateExit();
        }
        else if (currentState is DodgeState)
        {
            OnDodgeStateExit();
        }
        
        // Áp dụng damage và chuyển state
        enemy.TakeDamage(damage);
        hitState.HitID = id;
        enemy.StateMachine.ChangeState(hitState);
        actionTimer = 0f;
        
        // Khởi động lại AI decision loop
        StartAIDecisionLoop();
        
        LogDebug($"Force changed to Hit State from {currentState.GetType().Name}");
    }
    #endregion

    #region State Event Handlers
    private void OnIdleStateEnter()
    {
        canMakeDecision = true;
        LogDebug("Enemy entered Idle state");
    }

    private void OnAttackStateEnter()
    {
        LogDebug("Enemy entered Attack state");
    }

    private void OnDodgeStateEnter()
    {
        LogDebug("Enemy entered Dodge state");
    }

    private void OnAttackStateExit()
    {
        enemy.HitCharacters.Clear();
        LogDebug("Enemy exited Attack state");
    }

    private void OnDodgeStateExit()
    {
        isInvincible = false;
        LogDebug("Enemy exited Dodge state");
    }
    
    private void OnHitStateEnter()
    {
        LogDebug("Enemy entered Hit state");
    }

    private void OnHitStateExit()
    {
        LogDebug("Enemy exited Hit state");
    }
    #endregion

    #region Manual Testing
    private void HandleManualTesting()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            LogDebug("MANUAL ATTACK TEST!");
            ChangeToAttackState(Random.Range(0, 4));
        }
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            LogDebug("MANUAL DODGE TEST!");
            ChangeToDodgeState(Random.Range(0, 2));
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            LogDebug("MANUAL HIT TEST!");
            ChangeToHitState(Random.Range(0, 4), 10);
        }
    }
    #endregion

    #region Utility
    private void LogDebug(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[EnemyAI] {message}");
        }
    }

    // Public methods for external access
    public bool IsInvincible => isInvincible;
    public bool CanMakeDecision => canMakeDecision;
    public string CurrentStateName => enemy?.StateMachine?.CurrentState?.GetType().Name ?? "Unknown";
    
    // Gizmos for debugging
    private void OnDrawGizmosSelected()
    {
        if (enemy != null)
        {
            // Vẽ attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GetAttackRange());
            
            // Vẽ line đến player
            if (player != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, player.transform.position);
            }
        }
    }
    #endregion
}