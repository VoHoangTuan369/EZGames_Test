using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float minSwipeDistance = 50f;
    [SerializeField]
    private Vector2 delta;
    [SerializeField]
    private List<AnimationClip> animationClips;
    [SerializeField]
    private float actionTimer = 0f;
    [SerializeField]
    private float actionDuration = 1f;

    public Action<int> OnAttacking;
    public Action OnDodging;
    public Action<int> OnHitting;
    public Action OnResting;

    public Action OnAttackCanceled;
    
    public CharacterCollider CharacterCollider;
    public StateMachine StateMachine;
    private Character character;
    
    private IdleState idleState;
    private AttackState attackState;
    private DodgeState dodgeState;
    private HitState hitState;

    private void Start()
    {
        InitializeAnimationClip();
        InitializeStateMachine();
        OnHitting += ChangeToHitState;
    }
    
    private void InitializeAnimationClip()
    {
        Animator animator = StateMachine.Animator;
        
        var animatorController = animator.runtimeAnimatorController;
        if (!animatorController)
        {
            return;
        }
        
        AnimationClip[] clips = animatorController.animationClips;
    
        if (clips is { Length: > 0 })
        {
            animationClips.AddRange(clips);
        }
    }
    
    private void InitializeStateMachine()
    {
        idleState = new IdleState();
        attackState = new AttackState();
        dodgeState = new DodgeState();
        hitState = new HitState();

        idleState.OnStateEnter += OnIdleStateEnter;
        attackState.OnStateEnter += OnAttackStateEnter;
        dodgeState.OnStateEnter += OnDodgeStateEnter;
        
        idleState.OnStateExit += OnIdleStateExit;
        attackState.OnStateExit += OnAttackStateExit;
        dodgeState.OnStateExit += OnDodgeStateExit;
        
        StateMachine.ChangeState(idleState);
        character = StateMachine.gameObject.GetComponent<Character>();
    }

    private void OnDisable()
    {
        OnHitting -= ChangeToHitState;
        idleState.OnStateEnter -= OnIdleStateEnter;
        attackState.OnStateEnter -= OnAttackStateEnter;
        dodgeState.OnStateEnter -= OnDodgeStateEnter;
        idleState.OnStateExit -= OnIdleStateExit;
        attackState.OnStateExit -= OnAttackStateExit;
        dodgeState.OnStateExit -= OnDodgeStateExit;
    }
    
    private void Update()
    {
        HandleInput();
    }
    
    private void HandleInput()
    {
#if UNITY_EDITOR
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                endTouchPosition = Input.mousePosition;
                DetectGesture();
                delta = Vector2.zero;
            }
        }
        else
        {
            HandleMobileInput();
        }
#else
        HandleMobileInput();
#endif
        HandleActionTimer();
    }

    private void HandleMobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    DetectGesture();
                    delta = Vector2.zero;
                    break;
            }
        }
    }
    
    private void DetectGesture()
    {
        delta = endTouchPosition - startTouchPosition;

        if (delta.magnitude < minSwipeDistance)
        {
            Debug.Log("Tap detected");
            ChangeToAttackState(0);
            return;
        }

        float vertical = delta.y;
        float horizontal = delta.x;

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            if (horizontal > 0)
            {
                Debug.Log("Swipe Right → Attack");
                ChangeToAttackState(2);
            }
            else
            {
                Debug.Log("Swipe Left → Attack");
                ChangeToAttackState(3);
            }
        }
        else
        {
            if (vertical > 0)
            {
                Debug.Log("Swipe Up ↑ Attack");
                ChangeToAttackState(1);
            }
            else
            {
                Debug.Log("Swipe Down ↓ Defend");
                ChangeToDodgeState(0);
            }
        }
    }
    
    private void HandleActionTimer()
    {
        actionDuration = StateMachine.CurrentState switch
        {
            AttackState => GetLengthOfAttackClip(attackState.AttackID)/ character.Data.agility,
            DodgeState => GetLengthOfDodgeClip(dodgeState.DodgeID) / character.Data.agility,
            HitState => GetLengthOfHitClip(hitState.HitID),
            IdleState => animationClips.FirstOrDefault(obj => obj.name == "Idle")!.length,
            _ => actionDuration
        };
        actionTimer += Time.deltaTime;
        if (actionTimer >= actionDuration)
        {
            ChangeToIdleState();
        }
    }

    private float GetLengthOfAttackClip(int id)
    {
        return id switch
        {
            0 => animationClips.FirstOrDefault(obj => obj.name == "Head Punch")!.length,
            1 => animationClips.FirstOrDefault(obj => obj.name == "Stomach Punch")!.length,
            2 => animationClips.FirstOrDefault(obj => obj.name == "Kidney Punch Left")!.length,
            3 => animationClips.FirstOrDefault(obj => obj.name == "Kidney Punch Right")!.length,
            _ => 0f
        };
    }

    private float GetLengthOfDodgeClip(int id)
    {
        return id switch
        {
            0 or 1 => animationClips.FirstOrDefault(obj => obj.name == "Dodging Back")!.length,
            2 or 3 => animationClips.FirstOrDefault(obj => obj.name == "Dodging L")!.length,
            _ => 0f
        };
    }

    private float GetLengthOfHitClip(int id)
    {
        return id switch
        {
            0 => animationClips.FirstOrDefault(obj => obj.name == "Head Hit")!.length,
            1 => animationClips.FirstOrDefault(obj => obj.name == "Stomach Hit")!.length,
            2 or 3 => animationClips.FirstOrDefault(obj => obj.name == "Kidney Hit L")!.length,
            _ => 0f
        };
    }

    private void ChangeToAttackState(int id)
    {
        attackState.AttackID = id;
        StateMachine.ChangeState(attackState);
        actionTimer = 0f;
    }
    
    private void ChangeToDodgeState(int id)
    {
        dodgeState.DodgeID = id;
        StateMachine.ChangeState(dodgeState);
        actionTimer = 0f;
    }

    private void ChangeToHitState(int id)
    {
        hitState.HitID = id;
        StateMachine.ChangeState(hitState); ;
        actionTimer = 0f;
    }
    
    private void ChangeToIdleState()
    {
        StateMachine.ChangeState(idleState);
        actionTimer = 0f;
    }

    private void OnIdleStateEnter()
    {
        OnResting?.Invoke();
    }

    private void OnAttackStateEnter()
    {
        OnAttacking?.Invoke(attackState.AttackID);
    }

    private void OnDodgeStateEnter()
    {
        OnDodging?.Invoke();
    }
    
    private void OnIdleStateExit()
    {
        //OnResting?.Invoke();
    }

    private void OnAttackStateExit()
    {
        OnAttackCanceled?.Invoke();
        StateMachine.ChangeState(idleState);
    }

    private void OnDodgeStateExit()
    {
        StateMachine.ChangeState(idleState);
    }
}
