using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class About : MonoBehaviour
{
    [SerializeField]
    private SceneChanger sceneChanger = null;
    [SerializeField]
    private string nameMenuScene = "Menu";

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip audioClick = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }

    public void BackToMenu()
    {
        PlayAudio();

        if (nameMenuScene == "")
            return;

        sceneChanger.FadeToLevel(nameMenuScene);
    }

    public void PlayAudio()
    {
        AudioManager.Play(audioSource, audioClick);
    }
}
