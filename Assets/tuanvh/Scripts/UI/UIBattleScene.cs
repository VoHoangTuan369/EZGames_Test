using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleScene : MonoBehaviour
{
    [SerializeField] Slider playerHealthBar;
    [SerializeField] Slider enemyHealthBar;
    [SerializeField] Text levelText;
    [SerializeField] ResultPanel resultPanel;

    private Character player;
    private Character enemy;
    int level;

    private void Start()
    {
        player = GameManager.Instance.player;
        enemy = GameManager.Instance.enemy;
        level = PlayerPrefs.GetInt("CurrentLevel");

        InitUI();


        player.OnHealthChanged += OnPlayerHealthChanged;
        enemy.OnHealthChanged += OnEnemyHealthChanged;
    }

    void InitUI()
    {
        playerHealthBar.maxValue = player.Data.health;
        playerHealthBar.value = player.Data.health;

        enemyHealthBar.maxValue = enemy.Data.health;
        enemyHealthBar.value = enemy.Data.health;

        levelText.text = $"LEVEL {level}";
    }

    void OnPlayerHealthChanged(int currentHealth)
    {
        playerHealthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            GameManager.Instance.enemy.StateMachine.ChangeState(new WinState());
            //GameManager.Instance.player.StateMachine.ChangeState(new LoseState());
            resultPanel.ShowPanel(false);
        }
    }

    void OnEnemyHealthChanged(int currentHealth)
    {
        enemyHealthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            GameManager.Instance.player.StateMachine.ChangeState(new WinState());
            //GameManager.Instance.enemy.StateMachine.ChangeState(new LoseState());
            resultPanel.ShowPanel(true);
            GameManager.Instance.UpgradeLevel();
            GameManager.Instance.LoadLevel();
        }
    }

    private void OnDestroy()
    {
        player.OnHealthChanged -= OnPlayerHealthChanged;
        enemy.OnHealthChanged -= OnEnemyHealthChanged;
    }
}
