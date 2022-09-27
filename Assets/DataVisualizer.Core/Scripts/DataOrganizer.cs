using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DataVisualizer.Core.Scripts
{
    public class Node
    {
        public string Name;
        public List<Node> Children;
    }
    
    public class DataOrganizer : MonoBehaviour
    {
        [SerializeField] private CsvDataReader _csvDataReader;

        private void Start()
        {
            OrganizerData();
        }
        
        private async void OrganizerData()
        {
            Dictionary<string, List<string>> dataGroups = await _csvDataReader.GetData();
            Dictionary<string, List<string>> uniqueDataGroups = GetUniqueDataGroups(dataGroups);
            
            // Default layers for now
            string[] layers = dataGroups.Keys.ToArray();
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
    }
}