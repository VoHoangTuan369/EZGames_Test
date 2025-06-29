using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseLevelPanel : MonoBehaviour
{
    [SerializeField] LevelButton levelButton;
    [SerializeField] Transform buttonContaine;
    [SerializeField] LevelModeSO levelData;
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        int unlockLevel = PlayerPrefs.GetInt("UnlockLevel", 1);
        for (int i = 0; i < levelData.levels.Count; i++)
        {
            LevelButton newButton = Instantiate(levelButton, buttonContaine);
            bool isUnlocked = (levelData.levels[i].level) <= unlockLevel;
            newButton.InitButton(levelData.levels[i].level, isUnlocked);
        }
    }
}
