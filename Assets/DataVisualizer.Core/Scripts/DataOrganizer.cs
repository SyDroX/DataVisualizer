using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace DataVisualizer.Core.Scripts
{
    public class Node 
    {
        public Dictionary<string, Node> Children = new Dictionary<string, Node>();
    }

    public class DataOrganizer : MonoBehaviour
    {
        private bool _dataOrganized;
        private bool _dataRead;
        private string[] _layerOrder;
        private Dictionary<string, List<string>> _dataGroups;
        private Dictionary<string, List<string>> _uniqueDataGroups;
        private Dictionary<string, Node> _organizedData;
        
        [SerializeField] private CsvDataReader _csvDataReader;
        [SerializeField] private LayerOrderHandler _layerOrderHandler;

        public UnityEvent onLayersReordered;
        
        private void Start()
        {
            _layerOrderHandler.onLayersReordered.AddListener(OnLayersReordered);
        }
        
        private void OnLayersReordered(string[] newLayerOrder)
        {
            _dataOrganized = false;
            _layerOrder = newLayerOrder;
            onLayersReordered?.Invoke();
        }

        private async Task ReadData()
        {
            _dataGroups = await _csvDataReader.GetData();
            _uniqueDataGroups = GetUniqueDataGroups(_dataGroups);
            _layerOrder = _dataGroups.Keys.ToArray();
            _dataRead = true;
        }
        
        private Task OrganizerData()
        {
            // All data groups lengths are equal to the number of rows extracted from the CSV
            int rows = _dataGroups.First().Value.Count;
            _organizedData = new Dictionary<string, Node>();
            
            foreach (string dataGroup in _uniqueDataGroups[_layerOrder[0]])
            {
                _organizedData.Add(dataGroup, new Node());
            }
            
            for (int j = 0; j < rows; j++)
            {
                string currentRootName = _dataGroups[_layerOrder[0]][j];
                Node currentRootNode = _organizedData[currentRootName];
                
                for (int i = 1; i < _layerOrder.Length; i++)
                {
                    string currentNodeName = _dataGroups[_layerOrder[i]][j];
                    
                    if (!currentRootNode.Children.Keys.Contains(currentNodeName))
                    {
                        currentRootNode.Children.Add(currentNodeName, new Node());
                    }
                    
                    currentRootNode = currentRootNode.Children[currentNodeName];
                }
            }

            _dataOrganized = true;
            
            return Task.CompletedTask;
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
            if (!_dataRead)
            {
                await ReadData();
            }
            
            if (!_dataOrganized)
            {
                await OrganizerData();
            }

            return _organizedData;
        }
    }
}