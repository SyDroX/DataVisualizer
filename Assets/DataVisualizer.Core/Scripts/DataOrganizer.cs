using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace DataVisualizer.Core.Scripts
{
    public class Node 
    {
        public Dictionary<string, Node> Children = new Dictionary<string, Node>();
    }
    
    public class DataOrganizer : MonoBehaviour
    {
        private bool _dataOrganized;
        private Dictionary<string, Node> _organizedData;
        
        [SerializeField] private CsvDataReader _csvDataReader;

        private async Task OrganizerData()
        {
            Dictionary<string, List<string>> dataGroups = await _csvDataReader.GetData();
            Dictionary<string, List<string>> uniqueDataGroups = GetUniqueDataGroups(dataGroups);
            
            // All data groups lengths are equal to the number of rows extracted from the CSV
            int rows = dataGroups.First().Value.Count;
            
            // Default layers for now
            string[] layers = dataGroups.Keys.ToArray();
            
            _organizedData = new Dictionary<string, Node>();
            
            foreach (string dataGroup in uniqueDataGroups[layers[0]])
            {
                _organizedData.Add(dataGroup, new Node());
            }
            
            for (int j = 0; j < rows; j++)
            {
                string currentRootName = dataGroups[layers[0]][j];
                Node currentRootNode = _organizedData[currentRootName];
                
                for (int i = 1; i < layers.Length; i++)
                {
                    string currentNodeName = dataGroups[layers[i]][j];
                    
                    if (!currentRootNode.Children.Keys.Contains(currentNodeName))
                    {
                        currentRootNode.Children.Add(currentNodeName, new Node());
                    }
                    
                    currentRootNode = currentRootNode.Children[currentNodeName];
                }
            }

            _dataOrganized = true;
        }

        private Dictionary<string, List<string>> GetUniqueDataGroups(Dictionary<string, List<string>> dataGroups)
        {
            Dictionary<string, List<string>> uniqueDataGroups = new Dictionary<string, List<string>>();
            
            foreach (string groupName in dataGroups.Keys)
            {
                uniqueDataGroups.Add(groupName, dataGroups[groupName].Distinct().ToList());
            }

            return uniqueDataGroups;
        }

        public async Task<Dictionary<string, Node>> GetOrganizedData()
        {
            if (!_dataOrganized)
            {
                await OrganizerData();
            }

            return _organizedData;
        }
    }
}