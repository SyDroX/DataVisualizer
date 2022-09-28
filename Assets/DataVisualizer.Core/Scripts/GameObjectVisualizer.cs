using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameObjectVisualizer : MonoBehaviour
{
    private List<GameObject> _rootNodes = new List<GameObject>();
    private List<List<Vector2>> _layerPositions;
    private int _currentLayerNumber;

    [SerializeField] private GameObject _visualizerPrefab;
    [SerializeField] private DataOrganizer _dataOrganizer;
    [SerializeField] private GameObject _visualizationRoot;
    [SerializeField] private float _zDelta;
    [SerializeField] private float _layerSizeMultiplier = 1;
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
        _layerPositions.Add(new List<Vector2>(organizedData.Keys.Count));

        foreach (string nodeName in organizedData.Keys)
        {
            Vector2 position = await GeneratePositionInLayer(_layerPositions[_currentLayerNumber]);
            _layerPositions[_currentLayerNumber].Add(position);
            GameObject nodeInstance = CreateNodeGameObject(nodeName,
                _visualizationRoot,
                position,
                true);
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
        float range = await _dataOrganizer.GetUniqueDataGroupsCountInLayer(_currentLayerNumber) * _layerSizeMultiplier;
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

    private GameObject CreateNodeGameObject(string nodeName, GameObject parent, Vector2 position, bool isTopLevelNode)
    {
        Vector3 nodePosition = new Vector3(position.x, position.y, _currentLayerNumber * _zDelta);
        GameObject nodeInstance = Instantiate(_visualizerPrefab, nodePosition, Quaternion.identity, parent.transform);
        nodeInstance.name = nodeName;
        TextChanger textChanger = nodeInstance.GetComponent<TextChanger>();
        textChanger.ChangeText(nodeName);

        if (!isTopLevelNode)
        {
            DrawLineBetweenObjects(parent, nodeInstance);
        }

        return nodeInstance;
    }

    private void DrawLineBetweenObjects(GameObject source, GameObject target)
    {
        LineRenderer nodeLinkLine = target.AddComponent<LineRenderer>();
        nodeLinkLine.startWidth = 0.075f;
        nodeLinkLine.endWidth = 0.075f;
        nodeLinkLine.SetPosition(0, source.transform.position);
        nodeLinkLine.SetPosition(1, target.transform.position);
        nodeLinkLine.material.color = Color.black;
    }


    private async void VisualizeNode(Node rootNode, GameObject rootGameObject)
    {
        _currentLayerNumber++;

        if (_layerPositions.Count - 1 < _currentLayerNumber)
        {
            _layerPositions.Add(new List<Vector2>());
        }

        foreach (string childNodeName in rootNode.Children.Keys)
        {
            Vector2 position = await GeneratePositionInLayer(_layerPositions[_currentLayerNumber]);
            _layerPositions[_currentLayerNumber].Add(position);
            GameObject nodeInstance = CreateNodeGameObject(childNodeName,
                rootGameObject,
                position,
                false);
            VisualizeNode(rootNode.Children[childNodeName], nodeInstance);
        }

        _currentLayerNumber--;
    }

}
