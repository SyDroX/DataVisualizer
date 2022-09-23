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
        string[] rows = data.Split('\n');
        string[] headerColumns = rows[0].Split(',');
        
        for (int i = 1; i < rows.Length; i++) 
        {
            string[] row = rows[i].Split(',');

        }
    }
}