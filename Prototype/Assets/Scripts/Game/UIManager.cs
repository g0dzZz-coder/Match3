using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private TextMeshProUGUI textScore = null;
    [SerializeField]
    private TextMeshProUGUI textHearts = null;

    [Header("Panels")]
    [SerializeField]
    private GameObject pausePanel = null;
    [SerializeField]
    private GameObject tooltipPanel = null;
    [SerializeField]
    private GameObject winPanel = null;
    [SerializeField]
    private GameObject losePanel = null;

    [Header("Other")]
    [SerializeField]
    private string nameMenuScene = "Menu";
    [SerializeField]
    private string nameHighscoresScene = "Highscores";

    private GameManager gameMgr;

    public void Init(GameManager gameMgr, int startLives, int startScore)
    {
        this.gameMgr = gameMgr;

        UpdateStatsUI(startLives, startScore);
    }

    public void UpdateStatsUI(int newHearts, int newScore)
    {
        textHearts.text = newHearts.ToString();
        textScore.text = newScore.ToString();
    }

    public void Pause()
    {
        if (gameMgr.Paused)
        {
            UnPause();
            return;
        }

        Time.timeScale = 0;
        gameMgr.Paused = true;

        pausePanel.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        gameMgr.Paused = false;

        pausePanel.SetActive(false);
    }

    public void Restart()
    {
        gameMgr.RestartGame();
    }

    public void ShowGameOverPanel(bool win)
    {
        if (win)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
    }

    public void ShowTooltip()
    {
        gameMgr.Paused = true;
        tooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        gameMgr.Paused = false;
        tooltipPanel.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync(nameMenuScene);
    }

    public void ToHighScores()
    {
        SceneManager.LoadSceneAsync(nameHighscoresScene);
    }
}
