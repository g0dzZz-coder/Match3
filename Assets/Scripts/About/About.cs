using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class About : MonoBehaviour
{
    [SerializeField]
    private string nameMenuScene = "Menu";

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync(nameMenuScene);
    }
}
