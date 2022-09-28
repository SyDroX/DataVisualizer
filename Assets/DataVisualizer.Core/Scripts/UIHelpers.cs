using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DataVisualizer.Core.Scripts
{
    public static class UIHelpers
    {
        public static IEnumerator VerticalLayoutGroupCheat(VerticalLayoutGroup verticalLayoutGroup)
        {
            verticalLayoutGroup.enabled = true;
        
            yield return new WaitForEndOfFrame();
        
            verticalLayoutGroup.enabled = false;
        }
    }
}