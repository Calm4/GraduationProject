using App.Scripts.Animations;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Scripts.Buildings.UI
{
    public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform targetObject;
        [SerializeField] private AnimationsConfig animationsConfig;
        [SerializeField] private CanvasGroup canvasGroup;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (canvasGroup != null)
            {
                targetObject.gameObject.SetActive(true);
                canvasGroup.DOFade(1, animationsConfig.buildingDescriptionShowTime);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(0, animationsConfig.buildingDescriptionHideTime).OnComplete(() => targetObject.gameObject.SetActive(false));
            }
        }
    }
}