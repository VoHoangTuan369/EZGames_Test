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
        Character otherCharacter = other.GetComponentInParent<Character>();
        if (!otherCharacter)
        {
            return;
        }
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
                    character.HitCharacters.Add(otherCharacter);
                    otherCharacter.TakeDamage(character.Damage);
                }
                break;
            case CharacterType.Enemy:
                if (otherCharacter.type == CharacterType.Player)
                {
                    Debug.Log("Hit Player");
                    character.HitCharacters.Add(otherCharacter);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
