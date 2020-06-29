using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject PanelYesNo = null;

    [Header("Scenes")]
    [SerializeField]
    private string nameGameScene = "Game";
    [SerializeField]
    private string nameHighScoreTableScene = "HighScoreTable";
    [SerializeField]
    private string nameAboutScene = "About";

    public void StartGame()
    {
        LoadScene(nameGameScene);
    }

    public void ShownHighScoreTable()
    {
        LoadScene(nameHighScoreTableScene);
    }

    public void ShowAbout()
    {
        LoadScene(nameAboutScene);
    }

    public void Exit()
    {
        PanelYesNo.SetActive(!PanelYesNo.activeSelf);
    }

    public void ExitConfirmed()
    {
        Application.Quit();
    }

    private void LoadScene(string nameScene)
    {
        if (nameScene == "")
            return;

        SceneManager.LoadSceneAsync(nameScene);
    }
}
