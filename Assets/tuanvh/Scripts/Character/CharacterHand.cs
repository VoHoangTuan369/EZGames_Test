using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHand : MonoBehaviour
{
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
        Character otherCharacter = other.GetComponentInParent<Character>();
        if (character.HitCharacters.Contains(otherCharacter))
        {
            return;
        }

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
        otherCharacter.OnCharacterHasBeenHit?.Invoke(id, character.Damage);
    }
}
