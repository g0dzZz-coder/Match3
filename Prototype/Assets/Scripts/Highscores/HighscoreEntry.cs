
using System;

[System.Serializable]
public class HighscoreEntry
{
    public string Date;
    public int Score;
    public bool IsNew;

    public void SetDate(DateTime dateTime)
    {
        Date = string.Format("{0}.{1}.{2}", dateTime.Day, dateTime.Month, dateTime.Year);
    }
}
