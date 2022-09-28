using System.Collections;
using DataVisualizer.Core.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class LayerOrderUI : MonoBehaviour
{
    [SerializeField] private CsvDataReader _csvDataReader;
    [SerializeField] private GameObject _layersPanel;
    [SerializeField] private GameObject _layerSortingPanel;
    [SerializeField] private GameObject _layerPrefab;
    [SerializeField] private Button _showButton;
    [SerializeField] private Button _hideButton;

    private void Start()
    {
        ShowLayers();
    }

    private async void ShowLayers()
    {
        string[] layers = await _csvDataReader.GetLayers();
        
        foreach (string layer in layers)
        {
            GameObject layerGameObject = Instantiate(_layerPrefab, _layerSortingPanel.transform);
            layerGameObject.name = layer;
            layerGameObject.GetComponent<LayerUI>().Name = layer;
        }

        StartCoroutine(UIHelpers.VerticalLayoutGroupCheat(_layerSortingPanel.GetComponent<VerticalLayoutGroup>()));
    }
}
