using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelModeSO", menuName = "Game Data/Level Mode")]
public class LevelModeSO : ScriptableObject
{
    public List<Level> levels;
}

[Serializable]
public struct Level
{
    public int level;
    public CharacterStat enemyStat;
}
