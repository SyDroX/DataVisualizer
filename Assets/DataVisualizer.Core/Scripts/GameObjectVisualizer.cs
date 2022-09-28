using System.Collections.Generic;
using UnityEngine;

namespace DataVisualizer.Core.Scripts
{
    public class GameObjectVisualizer : MonoBehaviour
    {
        private List<GameObject> _rootNodes = new List<GameObject>();
        private List<List<Vector2>> _layerPositions;
        private int _currentLayerNumber;
        
        [SerializeField] private GameObject _visualizerPrefab;
        [SerializeField] private DataOrganizer _dataOrganizer;
        [SerializeField] private GameObject _visualizationRoot;
        [SerializeField] private float _zDelta;
        [SerializeField] private float _layerPlaneSizeMultiplier;
        [SerializeField] private float _minumDistanceBetweenObjects;
        
        private void Start()
        {
            VisualizeData();
            _dataOrganizer.onLayersReordered.AddListener(VisualizeData);
        }

        private async void VisualizeData()
        {
            DestroyVisualization();
            Dictionary<string, Node> organizedData = await _dataOrganizer.GetOrganizedData();
                    
            foreach (string nodeName in organizedData.Keys)
            {
                GameObject nodeInstance = CreateNodeGameObject(nodeName, _visualizationRoot.transform);
                _rootNodes.Add(nodeInstance);
                VisualizeNode(organizedData[nodeName], nodeInstance);
            }
        }

        private void DestroyVisualization()
        {
            foreach (GameObject rootNode in _rootNodes)
            {
                Destroy(rootNode);
            }
            
            _rootNodes.Clear();
            _currentLayerNumber = 0;
            _layerPositions = new List<List<Vector2>>();
        }

        private async Task<Vector2> GeneratePositionInLayer(List<Vector2> usedPositions)
        {
            float range = await _dataOrganizer.GetUniqueDataGroupsCountInLayer(_currentLayerNumber) * _layerPlaneSizeMultiplier;
            Vector2 position = new Vector2(Random.Range(-range, range), Random.Range(-range, range));
            bool objectsTooClose = ObjectsTooClose(usedPositions, position);

            while (objectsTooClose)
            {
                position = new Vector2(Random.Range(-range, range), Random.Range(-range, range));
                objectsTooClose = ObjectsTooClose(usedPositions, position);
            }

            return position;
        }

        private bool ObjectsTooClose(List<Vector2> usedPositions, Vector2 position)
        {
            foreach (Vector2 usedPosition in usedPositions)
            {
                if (Vector2.Distance(position, usedPosition) < _minumDistanceBetweenObjects)
                {
                    return true;
                }
            }

            return false;
        }
        
        private GameObject CreateNodeGameObject(string nodeName, Transform parent)
        {
            GameObject nodeInstance = Instantiate(_visualizerPrefab, parent);
            nodeInstance.name = nodeName;
            //TextChanger textChanger = nodeInstance.GetComponent<TextChanger>();
            //textChanger.ChangeText(nodeName);

            return nodeInstance;
        }
        
        private void VisualizeNode(Node rootNode, GameObject rootGameObject)
        {
            foreach (string childNodeName in rootNode.Children.Keys)
            {
                GameObject nodeInstance = CreateNodeGameObject(childNodeName, rootGameObject.transform);
                VisualizeNode(rootNode.Children[childNodeName], nodeInstance);
            }
        }
        
    }
}