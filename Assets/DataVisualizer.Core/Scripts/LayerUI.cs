using TMPro;
using UnityEngine;

public class LayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _layerText;

    private string _name;

    public string Name {
        get => _name;
        set
        {
            _name = value;
            _layerText.text = value;
        }
    }
}
