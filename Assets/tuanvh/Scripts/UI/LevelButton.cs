using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] Text levelText;
    int level;
    public void InitButton(int _level, bool isUnlocked)
    {
        level = _level;
        levelText.text = $"Level {level}";
        gameObject.GetComponent<Button>().interactable = isUnlocked;

        if (isUnlocked)
            gameObject.GetComponent<Button>().onClick.AddListener(PlayGame);
    }
    void PlayGame() 
    {
        StartLevel();
        StartCoroutine(ReloadSceneAfterDelay(1f));
    }
    IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameplayScene"); // hoặc tên scene cụ thể
    }
    void StartLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", level);
        PlayerPrefs.Save();
    }

}
