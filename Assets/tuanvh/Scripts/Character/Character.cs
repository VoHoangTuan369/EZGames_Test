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
    public StatSO stat;
    public Animator animator;
    private CharacterController controller;
    
    private StateMachine stateMachine;

    private float maxHealth;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private List<Character> hitCharacters = new List<Character>();

    public float Damage => damage;
    public List<Character> HitCharacters => hitCharacters;
    
    private void Awake()
    {
        stateMachine = new StateMachine(animator);
        stateMachine.InitializeState(new IdleState());
        if (type == CharacterType.Player)
        {
            controller = FindObjectOfType<CharacterController>();
        }
    }

    private void Start()
    {
        maxHealth = stat.characterStat.health;
        currentHealth = maxHealth;
        damage = stat.characterStat.damage;
        attackSpeed = stat.characterStat.attackSpeed;
        if (type != CharacterType.Player) return;
        controller.OnAttacking += OnCharacterAttacked;
        controller.OnDodging += OnCharacterDodged;
    }

    private void OnDisable()
    {
        if (type != CharacterType.Player) return;
        controller.OnAttacking -= OnCharacterAttacked;
        controller.OnDodging -= OnCharacterDodged;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    private void OnCharacterAttacked(int id)
    {
        AttackState attackState = new AttackState(id, hitCharacters);
        stateMachine.ChangeState(attackState);
    }
    
    private void OnCharacterDodged()
    {
        stateMachine.ChangeState(new DodgeState());
    }
}
