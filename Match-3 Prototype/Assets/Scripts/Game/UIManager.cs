using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip audioClick = null;

    [Header("Other")]
    [SerializeField]
    private float durationAnim = 0.2f;
    [SerializeField]
    private GameObject imageButtonRestart = null;
    [SerializeField]
    private SceneChanger sceneChanger = null;
    [SerializeField]
    private string nameMenuScene = "Menu";
    [SerializeField]
    private string nameHighscoresScene = "Highscores";

    private GameManager gameMgr;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Init(GameManager gameMgr, int startLives, int startScore)
    {
        this.gameMgr = gameMgr;

        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        UpdateStatsUI(startLives, startScore);
    }

    public void UpdateStatsUI(int newHearts, int newScore)
    {
        textHearts.text = newHearts.ToString();
        textScore.text = newScore.ToString();
    }

    public void Pause()
    {
        AudioManager.Play(audioSource, audioClick);

        ShowPanel(pausePanel);

        if (GameManager.Paused)
        {
            UnPause();
            return;
        }

        gameMgr.Pause();
    }

    public void UnPause()
    {
        AudioManager.Play(audioSource, audioClick);

        StartCoroutine(HidePanel(pausePanel));

        gameMgr.UnPause();
    }

    public void Restart()
    {
        AudioManager.Play(audioSource, audioClick);

        RotateRestartButton(durationAnim);

        gameMgr.RestartGame();
    }

    public void ShowGameOverPanel(bool win)
    {
        AudioManager.Play(audioSource, audioClick);

        if (win)
        {
            ShowPanel(winPanel);
        }
        else
        {
            ShowPanel(losePanel);
        }
    }

    public void ShowTooltip()
    {
        GameManager.Paused = true;

        ShowPanel(tooltipPanel);
    }

    public void HideTooltip()
    {
        AudioManager.Play(audioSource, audioClick);

        GameManager.Paused = false;

        StartCoroutine(HidePanel(tooltipPanel));
    }

    public void BackToMenu()
    {
        UnPause();

        LoadScene(nameMenuScene);
    }

    public void ToHighScores()
    {
        AudioManager.Play(audioSource, audioClick);

        LoadScene(nameHighscoresScene);
    }

    private void LoadScene(string nameScene)
    {
        AudioManager.Play(audioSource, audioClick);

        if (nameScene == "")
            return;

        sceneChanger.FadeToLevel(nameScene);
    }

    private void RotateRestartButton(float duration)
    {
        imageButtonRestart.transform.DORotate(new Vector3(0, 0, imageButtonRestart.transform.rotation.eulerAngles.z - 720), duration);
    }

    private void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
        DOTween.Sequence()
            .Append(panel.GetComponent<Image>().DOFade(1f, durationAnim));
    }

    private IEnumerator HidePanel(GameObject panel)
    {
        DOTween.Sequence()
               .Append(panel.GetComponent<Image>().DOFade(0f, durationAnim));

        yield return new WaitForSeconds(0f);

        panel.SetActive(false);
    }
}
