using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class CsvDataReader : MonoBehaviour
{
    private const string DefaultFileName = "Data.csv";

    private Dictionary<string, List<string>> _dataGroups;
    private bool _dataReady;
    
    [SerializeField] private string _filePath;

    private async Task ReadFile()
    {
        if (_filePath == string.Empty)
        {
            _filePath = Path.Combine(Application.persistentDataPath, DefaultFileName);
        }
        
        using StreamReader reader = new StreamReader(_filePath);
        string data = await reader.ReadToEndAsync();
        ConvertToData(data);
    }

    private void ConvertToData(string csvData)
    {
        string[] rows = csvData.Split("\r\n");
        string[] groups = rows[0].Split(',');
        _dataGroups = new Dictionary<string, List<string>>();

        foreach (string group in groups)
        {
            _dataGroups.Add(group, new List<string>());
        }

        for (int i = 1; i < rows.Length; i++)
        {
            string[] rowColumns = rows[i].Split(',');

            for (int j = 0; j < rowColumns.Length; j++)
            {
                _dataGroups[groups[j]].Add(rowColumns[j]);
            }
        }

        _dataReady = true;
    }

    public async Task<Dictionary<string, List<string>>> GetData()
    {
        if (!_dataReady)
        {
            await ReadFile();
        }

        return _dataGroups;
    }
}