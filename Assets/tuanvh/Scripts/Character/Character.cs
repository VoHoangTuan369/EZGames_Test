using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Player,
    Enemy
}

public class Character : MonoBehaviour
{
    public CharacterType type;
    public Animator animator;
    private PlayerController controller;
    
    private StateMachine stateMachine;

    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private List<Character> hitCharacters = new List<Character>();

    public int AttackID;
    public float Damage => data.damage;
    public float CurrentHealth => currentHealth;
    public List<Character> HitCharacters => hitCharacters;
    public StateMachine StateMachine => stateMachine;

    public CharacterStat Data { get => data; set => data = value; }

    public Action<int, int> OnGetHit;
    public Action<int> OnHealthChanged;

    CharacterStat data;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        stateMachine.InitializeState(new IdleState());
        controller = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        if (type != CharacterType.Player) return;
        controller.OnAttacking += OnCharacterAttacked;
        controller.OnDodging += OnCharacterDodged;
        controller.OnResting += OnCharacterRested;
        controller.OnAttackCanceled += OnCharacterAttackCanceled;
    }

    private void OnDisable()
    {
        if (type != CharacterType.Player) return;
        controller.OnAttacking -= OnCharacterAttacked;
        controller.OnDodging -= OnCharacterDodged;
        controller.OnResting -= OnCharacterRested;
        controller.OnAttackCanceled -= OnCharacterAttackCanceled;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke((int)currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead " + gameObject.name);
            
            StateMachine.ChangeState(new LoseState());
        }
    }

    private void OnCharacterAttacked(int attackID)
    {
        Debug.Log("Attackkk");
        AttackID = attackID;
        if (stateMachine.CurrentState is AttackState)
        {
            AttackState attackState = (AttackState)stateMachine.CurrentState;
            attackState.Speed = data.agility;
        }
        //AttackState attackState = new AttackState(id);
        //stateMachine.ChangeState(attackState);
    }
    
    private void OnCharacterDodged()
    {
        Debug.Log("Dodgeee");
        //stateMachine.ChangeState(new DodgeState(id));
    }

    private void OnCharacterRested()
    {
        Debug.Log("Resting");
        //stateMachine.ChangeState(new IdleState());
    }

    private void OnCharacterAttackCanceled()
    {
        Debug.Log("Ca");
        HitCharacters.Clear();
    }
    
    private IEnumerator DelayedHit(int id, float comingDamage)
    {
        float secondDelay;
        switch (id) 
        {
            case 0:
                secondDelay = 0.4f;
                break;
            case 3:
                secondDelay = 0.9f;
                break;
            default:
                secondDelay = 0.65f;
                break;
        }
        yield return new WaitForSeconds(secondDelay); // thời gian delay ở đây là 0.5 giây
        stateMachine.ChangeState(new HitState(){HitID = id});
        TakeDamage(Damage);
    }

    public void SetStat(CharacterStat _data)
    {
        data = _data;
        currentHealth = data.health;
    }
}
