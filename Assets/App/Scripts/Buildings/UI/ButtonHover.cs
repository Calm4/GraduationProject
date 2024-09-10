using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform targetObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetObject != null)
        {
            targetObject.gameObject.SetActive(true); 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetObject != null)
        {
            targetObject.gameObject.SetActive(false);
        }
    }
}
