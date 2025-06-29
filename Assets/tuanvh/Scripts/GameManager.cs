using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Character player;
    public Character enemy;
    public StatSO statSO;
    public LevelModeSO LevelModeSO;
    public int level;
    protected override void Awake()
    {
        base.Awake();
        LoadLevel();
        LoadDataFromSO();
    }
    void LoadDataFromSO() 
    {
        player.SetStat(statSO.characterStat);
        int clampedLevel = Mathf.Clamp(level, 0, LevelModeSO.levels.Count - 1);

        enemy.SetStat(LevelModeSO.levels[clampedLevel].enemyStat);
    }
    public void UpgradeLevel()
    {
        int unlockedLevel = Mathf.Max(PlayerPrefs.GetInt("UnlockLevel", 1), level + 1);
        PlayerPrefs.SetInt("UnlockLevel", unlockedLevel);
        PlayerPrefs.Save();
    }

    public void LoadLevel()
    {
        level = PlayerPrefs.GetInt("CurrentLevel");
        Debug.Log("Load level " + PlayerPrefs.GetInt("CurrentLevel"));
    }
}
