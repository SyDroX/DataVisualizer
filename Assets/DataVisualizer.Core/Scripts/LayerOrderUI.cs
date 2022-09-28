using UnityEngine;
using UnityEngine.UI;

public class LayersUI : MonoBehaviour
{
    [SerializeField] private CsvDataReader _csvDataReader;
    [SerializeField] private GameObject _layersPanel;
    [SerializeField] private GameObject _layerSortingPanel;
    [SerializeField] private GameObject _layerPrefab;
    [SerializeField] private Button _showButton;
    [SerializeField] private Button _hideButton;
    
    
    private void Start()
    {
        
    }

    private async void ShowLayers()
    {
        string[] layers = await _csvDataReader.GetLayers();
        
        foreach (string layer in layers)
        {
            GameObject layerGameObject = Instantiate(_layerPrefab, _layerSortingPanel.transform);
            
        }   
        
    }
}
