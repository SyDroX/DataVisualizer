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
    [SerializeField] private LayerOrderHandler _layerOrder;
    [SerializeField] private Vector2 _panelHiddenPosition;
    private void Start()
    {
        _showButton.onClick.AddListener(OnShowButtonClick);
        _hideButton.onClick.AddListener(OnHideButtonClick);
        ShowLayers();
    }
    private void OnShowButtonClick()
    {
        _layersPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        _hideButton.gameObject.SetActive(true);
        _showButton.gameObject.SetActive(false);
    }

    private void OnHideButtonClick()
    {
        _layersPanel.GetComponent<RectTransform>().anchoredPosition = _panelHiddenPosition;
        _hideButton.gameObject.SetActive(false);
        _showButton.gameObject.SetActive(true);
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

        StartCoroutine(BehaviourHelpers.ToggleComponentBehaviour(_layerSortingPanel.GetComponent<VerticalLayoutGroup>()));
    }

    private void OnLayersReordered()
    {
        string[] newLayerOrder = new string[_layerSortingPanel.transform.childCount];
        
        for (int i = 0; i < _layerSortingPanel.transform.childCount; i++)
        {
            newLayerOrder[i] = _layerSortingPanel.transform.GetChild(i).name;
        }
        
        _layerOrder.onLayersReordered?.Invoke(newLayerOrder);
    }
    
}
