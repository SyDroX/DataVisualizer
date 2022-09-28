﻿using UnityEngine;
using UnityEngine.EventSystems;


public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private GameObject _mainContent;
    private Vector3 _currentPosition;
    private int _totalChild;

    [SerializeField] private int _minimumReorderDistance = 10;
    
    public RectTransform currentTransform;

    public void OnPointerDown(PointerEventData eventData)
    {
        _currentPosition = currentTransform.position;
        _mainContent = currentTransform.parent.gameObject;
        _totalChild = _mainContent.transform.childCount;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 currentPosition = currentTransform.position;
        currentPosition = new Vector3(currentPosition.x, eventData.position.y, currentPosition.z);
        currentTransform.position = currentPosition;

        for (int i = 0; i < _totalChild; i++)
        {
            if (i != currentTransform.GetSiblingIndex())
            {
                Transform otherTransform = _mainContent.transform.GetChild(i);
                int distance = (int) Vector3.Distance(currentTransform.position,
                    otherTransform.position);

                if (distance <= _minimumReorderDistance)
                {
                    Vector3 otherPosition = otherTransform.position;
                    Vector3 otherTransformOldPosition = otherPosition;
                    otherPosition = new Vector3(otherPosition.x, _currentPosition.y, otherPosition.z);
                    otherTransform.position = otherPosition;
                    Vector3 newPosition = currentTransform.position;
                    newPosition = new Vector3(newPosition.x, otherTransformOldPosition.y, newPosition.z);
                    currentTransform.position = newPosition;
                    currentTransform.SetSiblingIndex(otherTransform.GetSiblingIndex());
                    _currentPosition = newPosition;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        currentTransform.position = _currentPosition;
    }
}