using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandType
{
    Left, Right
}

public class CharacterHand : MonoBehaviour
{
    public HandType handType;
    private Character character;
    
    private void Start()
    {
        character = GetComponentInParent<Character>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (character.StateMachine.CurrentState is not AttackState)
        {
            return;
        }

        HandType triggerHandType = HandType.Left;
        
        if (character.StateMachine.CurrentState is AttackState attackState)
        {
            switch (attackState.AttackID)
            {
                case 0:
                    triggerHandType = HandType.Right;
                    break;
                case 1:
                    triggerHandType = HandType.Left;
                    break;
                case 2:
                    triggerHandType = HandType.Left;
                    break;
                case 3:
                    triggerHandType = HandType.Right;
                    break;
            }
        }

        if (triggerHandType != handType)
        {
            Debug.Log("Hit other hand: " + handType);
            return;
        }
        
        Character otherCharacter = other.GetComponentInParent<Character>();
        if (character.HitCharacters.Contains(otherCharacter))
        {
            return;
        }

        character.StateMachine.Animator.speed = 1;
        
        switch (character.type)
        {
            case CharacterType.Player:
                if (otherCharacter.type == CharacterType.Enemy)
                {
                    Debug.Log("Hit Enemy");
                    HitHandle(otherCharacter);
                }
                break;
            case CharacterType.Enemy:
                if (otherCharacter.type == CharacterType.Player)
                {
                    Debug.Log("Hit Player");
                    HitHandle(otherCharacter);
                }
                break;
        }
    }

    private void HitHandle(Character otherCharacter)
    {
        int id = character.AttackID;
        character.HitCharacters.Add(otherCharacter);
        otherCharacter.OnGetHit?.Invoke(id, (int)character.Damage);
    }
}
