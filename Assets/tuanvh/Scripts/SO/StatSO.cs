using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ CreateAssetMenu(fileName = "NewStatSO", menuName = "Game Data/Stat Data")]
public class StatSO : ScriptableObject
{
    public CharacterStat characterStat;
}

[Serializable]
public class CharacterStat
{
    public float damage;
    public float health;
    public float agility;
}
