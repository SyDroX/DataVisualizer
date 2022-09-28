using System.Collections;
using UnityEngine;
public static class BehaviourHelpers
{
    // This a generic component toggle
    // Its useful for Unity UI components, since they update themselves at the next frame thus require a WaitForEndOfFrame
    // Disabling the VerticalLayoutGroup component allows smooth dragging inside the panel
    // enabling the VerticalLayoutGroup snaps all the elements inside to align
    public static IEnumerator ToggleComponentBehaviour(Behaviour componentBehaviour)
    {
        componentBehaviour.enabled = true;
    
        yield return new WaitForEndOfFrame();
    
        componentBehaviour.enabled = false;
    }

}