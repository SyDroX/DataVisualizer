using TMPro;
using UnityEngine;

public class TextChanger : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] _targetTexts;

    public void ChangeText(string newText)
    {
        foreach (TextMeshPro targetText in _targetTexts)
        {
            targetText.text = newText;
        }
    }
}
