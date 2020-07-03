using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum Complexity
{
    Easy,
    Hard
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int hearts = 10;
    public int GetHearts => hearts;
    [SerializeField]
    private int score = 0;
    public int GetScore => score;

    [SerializeField]
    private int tileReward = 100;
    public int GetTileReward => tileReward;
    [SerializeField, Range(1.0f, 3.0f)]
    private float comboMultiplier = 1.0f;
    public float GetComboMuiltiplier => comboMultiplier;

    [SerializeField]
    private Complexity complexity = Complexity.Easy;
    public Complexity GetComplexity => complexity;

    [SerializeField, Range(5f, 30f)]
    private float promptFrequency = 10f;
    public float GetPromptFrequency => promptFrequency;
    [SerializeField]
    private int promptAccuracy = 5;
    [SerializeField]
    public int GetPromptAccuracy => promptAccuracy;

    public static bool Paused { get; set; }

    public static bool GameIsOver { get; set; }

    private int startLives = 10;
    private int startScore = 0;

    private List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>();

    private Board board;
    private UIManager uIManager;

    private void Awake()
    {
        board = FindObjectOfType<Board>();
        uIManager = FindObjectOfType<UIManager>();

        UnPause();
    }

    void Start()
    {
        startLives = hearts;
        startScore = score;

        // Инициализация копонентов.
        RestartGame();

        if (PlayerPrefs.HasKey("numberOfGames"))
        {
            int numberOfGames = PlayerPrefs.GetInt("numberOfGames");
            PlayerPrefs.SetInt("numberOfGames", numberOfGames++);
        }
        else
        {
            uIManager.ShowTooltip();
            PlayerPrefs.SetInt("numberOfGames", 1);
        }
        PlayerPrefs.Save();
    }

    public void UpdateStats(int hearts, int score)
    {
        this.score = score;

        if (hearts < 1)
        {
            this.hearts = 0;
            GameOver();
        }
        else
        {
            this.hearts = hearts;
        }

        uIManager.UpdateStatsUI(this.hearts, this.score);
    }

    public void GameOver()
    {
        GameIsOver = true;

        bool isInHighScores = IsInHighScores();
        if (isInHighScores)
        {
            SaveResult();
        }

        uIManager.ShowGameOverPanel(isInHighScores);
    }

    public void RestartGame()
    {
        UnPause();

        board.Init(this);
        uIManager.Init(this, startLives, startScore);
    }

    // Проверка попадания результата в список лучших.
    public bool IsInHighScores()
    {
        if (score == 0)
            return false;

        bool among = true;

        if (PlayerPrefs.HasKey("highscoreTable"))
        {
            string jsonString = PlayerPrefs.GetString("highscoreTable");
            Highscores highscores = new Highscores();
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
            highscoreEntries = highscores.HighscoreEntries;

            if (highscoreEntries.Count < 9)
                return true;

            highscoreEntries.Sort((entry1, entry2) => (entry2.Score.CompareTo(entry1.Score)));

            if (score > highscoreEntries[highscoreEntries.Count - 1].Score)
                among = true;
            else
                among = false;
        }

        return among;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        Paused = false;
        GameIsOver = false;
    }

    public void Pause()
    {
        //Time.timeScale = 0;
        Paused = true;
    }

    // Сохранение результата.
    private void SaveResult()
    {
        HighscoreEntry result = new HighscoreEntry() { Score = score, IsNew = true };
        result.SetDate(DateTime.Now);
        highscoreEntries.Add(result);

        Highscores highscores = new Highscores { HighscoreEntries = highscoreEntries };
        string jsonString = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", jsonString);
        PlayerPrefs.Save();
    }
}
