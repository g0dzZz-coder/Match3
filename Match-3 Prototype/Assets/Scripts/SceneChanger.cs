using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private Animator animator = null;

    private string sceneToLoad = "";

    public void FadeToLevel(string newScene)
    {
        sceneToLoad = newScene;
        animator.SetTrigger("FadeOut");
    }
    
    public void OnFadeComplete()
    {
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
