using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Highscores
{
    public List<HighscoreEntry> HighscoreEntries;
}

public class HighscoresManager : MonoBehaviour
{
    [SerializeField, Range(5, 10)]
    private int numberOfLines = 10;
    [SerializeField]
    private bool deletePlayerPrefs = false;
    [SerializeField]
    private TextAsset fileCSV = null;
    [SerializeField, Tooltip("Vertical Layout Group")]
    private Transform entryContainer = null;
    [SerializeField, Tooltip("Префаб записи")]
    private Transform entryTemplate = null;

    [Space(10)]
    [SerializeField]
    private SceneChanger sceneChanger = null;
    [SerializeField]
    private string nameMenuScene = "Menu";

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip audioClick = null;

    [Space(10)]
    [SerializeField]
    private bool debug = false;

    private List<HighscoreEntry> highscoreEntryList = null;

    private void Awake()
    {
        highscoreEntryList = new List<HighscoreEntry>();

        if (deletePlayerPrefs)
            PlayerPrefs.DeleteAll();

        if (PlayerPrefs.HasKey("highscoreTable"))
        {
            if (debug)
                Debug.Log("using PlayerPrefs");

            LoadHighscores();
        }
        else
        {
            if (debug)
                Debug.Log("using CSV");

            highscoreEntryList = ReadCSV.ReadCSVFile(fileCSV.name, debug);
            SaveHighscores();
        }

        SortHighscores();
        RemoveUnnecessary();
        SpawnLines(highscoreEntryList);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }

    public void BackToMenu()
    {
        AudioManager.Play(audioSource, audioClick);

        if (nameMenuScene == "")
            return;

        ResetNewResult();
        sceneChanger.FadeToLevel(nameMenuScene);
    }

    private void SaveHighscores()
    {
        Highscores highscores = new Highscores { HighscoreEntries = highscoreEntryList };
        string jsonString = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", jsonString);
        PlayerPrefs.Save();

        if (debug)
            Debug.Log(PlayerPrefs.GetString("highscoreTable"));
    }

    private void LoadHighscores()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = new Highscores();
        highscores = JsonUtility.FromJson<Highscores>(jsonString);
        highscoreEntryList = highscores.HighscoreEntries;

        if (debug)
            Debug.Log(PlayerPrefs.GetString("highscoreTable"));
    }

    private void AddHighscoreEntry(string date, int score)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { Date = date, Score = score };
        highscoreEntryList.Add(highscoreEntry);

        SaveHighscores();
    }

    // Сортировка по счёту.
    private void SortHighscores()
    {
        highscoreEntryList.Sort((entry1, entry2) => (entry2.Score.CompareTo(entry1.Score)));
    }

    private void RemoveUnnecessary()
    {
        if (highscoreEntryList.Count > numberOfLines)
        {
            for (int i = numberOfLines; i < highscoreEntryList.Count; i++)
            {
                highscoreEntryList.RemoveAt(i);
            }
        }
    }

    private void SpawnLines(List<HighscoreEntry> highscoreEntries)
    {
        for (int i = 0; i < highscoreEntries.Count; i++)
        {
            if (i > numberOfLines)
                return;

            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            HighscoreEntryComponent entryComponent = entryTransform.GetComponent<HighscoreEntryComponent>();

            int place = i + 1;

            entryComponent.SetValues(
                place.ToString(),
                highscoreEntries[i].Date,
                highscoreEntries[i].Score.ToString(),
                highscoreEntries[i].IsNew);
        }
    }

    private void ResetNewResult()
    {
        for (int i = 0; i < highscoreEntryList.Count; i++)
        {
            highscoreEntryList[i].IsNew = false;
        }

        SaveHighscores();
    }
}
