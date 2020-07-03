using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    [SerializeField]
    private string url = "";

    public void OpenUrl()
    {
        if (url == "")
            return;

        Application.OpenURL(url);
    }    
}
