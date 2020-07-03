using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private SceneChanger sceneChanger = null;
    [SerializeField]
    private string nameMenuScene = "Menu";

    [SerializeField]
    private Slider sliderVolume = null;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip audioClick = null;
    [SerializeField]
    private AudioClip audioSlider = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }

    private void Awake()
    {
        sliderVolume.value = AudioManager.Volume;
    }

    public void BackToMenu()
    {
        AudioManager.Play(audioSource, audioClick);

        if (nameMenuScene == "")
            return;

        sceneChanger.FadeToLevel(nameMenuScene);
    }

    public void DisableAudio()
    {
        sliderVolume.value = 0f;
        AudioManager.UpdateVolume(sliderVolume.value);
        AudioManager.Play(audioSource, audioSlider, false, true);
    }

    public void EnableAudio()
    {
        sliderVolume.value = 1f;
        AudioManager.UpdateVolume(sliderVolume.value);
        AudioManager.Play(audioSource, audioSlider, false, true);
    }

    public void CheckSlider()
    {
        AudioManager.UpdateVolume(sliderVolume.value);
        audioSource.volume = AudioManager.Volume;

        AudioManager.Play(audioSource, audioSlider, false, true);
    }
}
