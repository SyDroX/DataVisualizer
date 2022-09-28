﻿using System.Collections.Generic;
using UnityEngine;

namespace DataVisualizer.Core.Scripts
{
    public class GameObjectVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject _visualizerPrefab;
        [SerializeField] private DataOrganizer _dataOrganizer;
        [SerializeField] private GameObject _visualizationRoot;

        private void Start()
        {
            VisualizeData();
        }

        private async void VisualizeData()
        {
            Dictionary<string, Node> organizedData = await _dataOrganizer.GetOrganizedData();
                    
            foreach (string nodeName in organizedData.Keys)
            {
                GameObject nodeInstance = CreateNodeGameObject(nodeName, _visualizationRoot.transform);

                foreach (string childNodeName in organizedData[nodeName].Children.Keys)
                {
                    GameObject childNodeInstance = CreateNodeGameObject(childNodeName, nodeInstance.transform);
                    VisualizeNode(organizedData[nodeName].Children[childNodeName], childNodeInstance);
                }
            }
        }

        private GameObject CreateNodeGameObject(string nodeName, Transform parent)
        {
            GameObject nodeInstance = Instantiate(_visualizerPrefab, parent);
            nodeInstance.name = nodeName;
            TextChanger textChanger = nodeInstance.GetComponent<TextChanger>();
            textChanger.ChangeText(nodeName);

            return nodeInstance;
        }
        
        private void VisualizeNode(Node rootNode, GameObject rootGameObject)
        {
            foreach (string childNodeName in rootNode.Children.Keys)
            {
                GameObject nodeInstance = CreateNodeGameObject(childNodeName, rootGameObject.transform);
                
                foreach (Node childNode in rootNode.Children.Values)
                {
                    VisualizeNode(childNode, nodeInstance);
                }
            }
        }
        
    }
}