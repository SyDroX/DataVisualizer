using System.Collections.Generic;
using UnityEngine;

namespace DataVisualizer.Core.Scripts
{
    public class GameObjectVisualizer : MonoBehaviour
    {
        private List<GameObject> _rootNodes = new List<GameObject>();

        [SerializeField] private GameObject _visualizerPrefab;
        [SerializeField] private DataOrganizer _dataOrganizer;
        [SerializeField] private GameObject _visualizationRoot;
        
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