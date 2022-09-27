using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DataVisualizer.Core.Scripts
{
    public class Node 
    {
        public Dictionary<string, Node> Children = new Dictionary<string, Node>();
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
            
            // All data groups lengths are equal to the number of rows extracted from the CSV
            int rows = dataGroups.First().Value.Count;
            
            // Default layers for now
            string[] layers = dataGroups.Keys.ToArray();
            
            Dictionary<string, Node> rootNodes2 = new Dictionary<string, Node>();
            
            foreach (string dataGroup in uniqueDataGroups[layers[0]])
            {
                rootNodes2.Add(dataGroup, new Node());
            }
            
            for (int j = 0; j < rows; j++)
            {
                string currentRootName = dataGroups[layers[0]][j];
                Node currentRootNode = rootNodes2[currentRootName];
                
                for (int i = 1; i < layers.Length; i++)
                {
                    bool hasChild = false;
                    string currentNodeName = dataGroups[layers[i]][j];
                    
                    foreach (string childNode in currentRootNode.Children.Keys)
                    {
                        string parentNodeName = dataGroups[layers[i - 1]][j];
                        
                        if (childNode == currentNodeName && currentRootName == parentNodeName)
                        {
                            hasChild = true;
                        }
                    }

                    if (!hasChild)
                    {
                        currentRootNode.Children.Add(dataGroups[layers[i]][j], new Node());
                    }
                    
                    currentRootNode = currentRootNode.Children[currentNodeName];
                }
            }
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