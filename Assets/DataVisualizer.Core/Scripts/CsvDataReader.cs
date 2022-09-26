using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CsvDataReader : MonoBehaviour
{
    private const string DefaultFileName = "Data.csv";
    [SerializeField] private string filePath;

    private void Start()
    {
        if (filePath == string.Empty)
        {
            filePath = Path.Combine(Application.persistentDataPath ,DefaultFileName);
        }
        
        ReadFile();
    }

    private async void ReadFile()
    {
        using StreamReader reader = new StreamReader(filePath);
        string data = await reader.ReadToEndAsync();
        ConvertToData(data);
    }

    private void ConvertToData(string data)
    {
        string[] rows = data.Split('\r', '\n');
        string[] headers = rows[0].Split(',');
        
        Dictionary<string, List<string>> formattedData = new Dictionary<string, List<string>>();
        
        for (int i = 0; i < headers.Length; i++) 
        {
            formattedData.Add(headers[i], new List<string>());
        }
        
        for (int i = 1; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split(',');
            
            for (int j = 0; j < columns.Length; j++)
            {
                formattedData[headers[j]].Add(columns[j]);
            }
        }
        
        
    }
}