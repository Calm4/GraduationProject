using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Scripts.Buildings.UI
{
    public class ClickOutsideDetector : MonoBehaviour
    {
        [SerializeField] private BuildingPanelUI panel;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                    panel.GetComponent<RectTransform>(),
                    eventData.position,
                    eventData.pressEventCamera))
            {
                panel.HidePanel();
            }
        } 
    }
}