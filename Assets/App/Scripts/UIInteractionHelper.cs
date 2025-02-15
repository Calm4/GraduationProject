using UnityEngine;

namespace App.Scripts
{
    public static class UIInteractionHelper
    {
        public static bool IsPointerOverUIElement(RectTransform rectTransform, Vector2 pointerPosition)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pointerPosition, null);
        }
    }
}