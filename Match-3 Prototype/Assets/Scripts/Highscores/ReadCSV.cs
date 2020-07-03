using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class ReadCSV
{
    public static List<HighscoreEntry> ReadCSVFile(string pathFile, bool debug)
    {
        List<HighscoreEntry> list = new List<HighscoreEntry>();

        TextAsset dataTable = Resources.Load<TextAsset>(pathFile);

        string[] data = dataTable.text.Split(new char[] { '\n' });
        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            HighscoreEntry dataRow = new HighscoreEntry();
            dataRow.Date = row[0];
            int.TryParse(row[1], out dataRow.Score);
            dataRow.IsNew = false;

            list.Add(dataRow);
        }

        if (debug)
        {
            foreach (HighscoreEntry dr in list)
            {
                Debug.Log(dr.Date+ ", " + dr.Score);
            }
        }

        return list;
    }
}
