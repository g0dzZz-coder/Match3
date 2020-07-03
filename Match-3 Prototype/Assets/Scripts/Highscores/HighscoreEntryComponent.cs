using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreEntryComponent : MonoBehaviour
{
    [SerializeField]
    private Image backgroundImage = null;

    [SerializeField]
    private TextMeshProUGUI placeText = null;
    [SerializeField]
    private TextMeshProUGUI dateText = null;
    [SerializeField]
    private TextMeshProUGUI scoreText = null;

    public void SetValues(string place, string date, string score, bool isNew)
    {
        placeText.text = place;
        dateText.text = date;
        scoreText.text = score;

        if (isNew)
            backgroundImage.color = Color.green;
    }

    public void HighlightEntry()
    {
        backgroundImage.color = Color.yellow;
    }
}
