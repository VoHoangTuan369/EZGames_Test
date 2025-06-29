using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] Text resultText, nextBtnText;
    [SerializeField] Button nextBtn;

    string winner = "WINNER!!";
    string loser = "LOSER";
    string next = "Next";
    string back = "Back";
    public void ShowPanel(bool isWinner) 
    {
        if (isWinner)
        {
            resultText.text = winner;
            nextBtnText.text = next;
        }
        else 
        {
            resultText.text = loser;
            nextBtnText.text = back;
        }
        nextBtn.onClick.AddListener(() => { StartCoroutine(ReloadSceneAfterDelay(2f)); });
        gameObject.SetActive(true);
    }
    IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("LevelScene");
    }

}
