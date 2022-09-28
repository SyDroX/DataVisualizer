using DataVisualizer.Core.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LayerOrderUI : MonoBehaviour
{
    [SerializeField] private CsvDataReader _csvDataReader;
    [SerializeField] private GameObject _layersPanel;
    [SerializeField] private GameObject _layerSortingPanel;
    [SerializeField] private GameObject _layerPrefab;
    [SerializeField] private Button _showButton;
    [SerializeField] private Button _hideButton;

    public UnityEvent<string[]> onLayersReordered;
    
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
            layerGameObject.GetComponent<VerticalDraggable>().onDragged.AddListener(OnLayersReordered);
        }

        StartCoroutine(UIHelpers.VerticalLayoutGroupCheat(_layerSortingPanel.GetComponent<VerticalLayoutGroup>()));
    }

    private void OnLayersReordered()
    {
        string[] newLayerOrder = new string[_layerSortingPanel.transform.childCount];
        
        for (int i = 0; i < _layerSortingPanel.transform.childCount; i++)
        {
            newLayerOrder[i] = _layerSortingPanel.transform.GetChild(i).name;
        }
        
        onLayersReordered?.Invoke(newLayerOrder);
    }
    
}
