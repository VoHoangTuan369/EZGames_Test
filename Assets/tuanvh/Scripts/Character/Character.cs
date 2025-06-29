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

    public Action<int, float> OnCharacterHasBeenHit;
    public Action<int> OnHealthChanged;

    CharacterStat data;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        stateMachine.InitializeState(new IdleState());
        if (type == CharacterType.Player)
        {
            controller = FindObjectOfType<PlayerController>();
        }
    }

    private void Start()
    {
        OnCharacterHasBeenHit += OnCharHasBeenHit;
        if (type != CharacterType.Player) return;
        controller.OnAttacking += OnCharacterAttacked;
        controller.OnDodging += OnCharacterDodged;
        
    }

    private void OnDisable()
    {
        OnCharacterHasBeenHit -= OnCharHasBeenHit;
        if (type != CharacterType.Player) return;
        controller.OnAttacking -= OnCharacterAttacked;
        controller.OnDodging -= OnCharacterDodged;
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

    private void OnCharacterAttacked(int id)
    {
        AttackID = id;
        AttackState attackState = new AttackState(id, hitCharacters);
        stateMachine.ChangeState(attackState);
    }
    
    private void OnCharacterDodged(int id)
    {
        stateMachine.ChangeState(new DodgeState(id));
    }

    private void OnCharHasBeenHit(int id, float comingDamage)
    {
        //Debug.Log("Hitttt");
        StartCoroutine(DelayedHit(id, comingDamage));
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
        stateMachine.ChangeState(new HitState(id));
        TakeDamage(Damage);
    }

    public void SetStat(CharacterStat _data)
    {
        data = _data;
        currentHealth = data.health;
    }
}
