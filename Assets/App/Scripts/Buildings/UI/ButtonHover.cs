using System.Collections;
using System.Collections.Generic;
using App.Scripts.Animations;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform targetObject;
    [SerializeField] private AnimationsConfig animationsConfig;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetObject != null)
        {
            targetObject.gameObject.SetActive(true);
            targetObject.DOScale(1,animationsConfig.buildingDescriptionTime).SetEase(Ease.InCirc); 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetObject != null)
        {
            targetObject.DOScale(0,animationsConfig.buildingDescriptionTime).SetEase(Ease.OutCirc).OnComplete(() => targetObject.gameObject.SetActive(false));
        }
    }
}
