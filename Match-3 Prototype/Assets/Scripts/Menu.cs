using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject PanelYesNo = null;

    [Header("Scenes")]
    [SerializeField]
    private SceneChanger sceneChanger = null;
    [SerializeField]
    private string nameSeneGame = "Game";
    [SerializeField]
    private string nameSceneHighscores = "Highscores";
    [SerializeField]
    private string nameSceneAsd = "Ads";
    [SerializeField]
    private string nameSceneAbout = "About";
    [SerializeField]
    private string nameSceneSettings = "Settings";

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip audioClick = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PanelYesNo.activeSelf)
                ExitConfirmed();
            else
                Exit();
        }
    }

    public void StartGame()
    {
        LoadScene(nameSeneGame);
    }

    public void ShownHighScoreTable()
    {
        LoadScene(nameSceneHighscores);
    }

    public void ShowAds()
    {
        LoadScene(nameSceneAsd);
    }

    public void ShowAbout()
    {
        LoadScene(nameSceneAbout);
    }

    public void ShowSettings()
    {
        LoadScene(nameSceneSettings);
    }

    public void Exit()
    {
        PanelYesNo.SetActive(!PanelYesNo.activeSelf);
        AudioManager.Play(audioSource, audioClick);
    }

    public void ExitConfirmed()
    {
        AudioManager.Play(audioSource, audioClick);
        Application.Quit();
    }

    private void LoadScene(string nameScene)
    {
        AudioManager.Play(audioSource, audioClick);

        if (nameScene == "")
            return;

        sceneChanger.FadeToLevel(nameScene);
    }
}
