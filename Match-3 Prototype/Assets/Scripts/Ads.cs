using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour
{
    [SerializeField]
    private bool testMode = true;

#if UNITY_IOS
    private string gameID = "3692044";
#elif UNITY_ANDROID
    private string gameID = "3692045";
#endif

    private void Start()
    {
        Advertisement.Initialize(gameID, testMode);
    }

    public void ShowAds()
    {
        Advertisement.Show();
    }
}
