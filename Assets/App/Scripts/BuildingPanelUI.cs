using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts
{
    public class BuildingPanelUI : MonoBehaviour
    {
        private RectTransform _goRectTransform;
    
        [Title("Animation Params")] 
        [SerializeField] private float animationTime;

        public event Action<float> OnButtonPressed;

        private bool _isHide;
        private Vector2 _panelStartPosition;

        private void Start()
        {
            _goRectTransform = GetComponent<RectTransform>();
            _panelStartPosition = _goRectTransform.anchoredPosition;
        }

        public void ShowOrHideBuildingsPanel()
        {
            OnButtonPressed?.Invoke(animationTime);

            if (_isHide)
            {
                // Вернуть панель на исходную позицию
                _goRectTransform.DOAnchorPos(_panelStartPosition, animationTime).SetEase(Ease.InOutSine);
            }
            else
            {
                // Рассчитать позицию, при которой панель полностью скрыта за нижней границей экрана
                Vector2 hidePosition = CalculateHidePosition();
                _goRectTransform.DOAnchorPos(hidePosition, animationTime).SetEase(Ease.InCirc);
            }

            _isHide = !_isHide;
        }

        private Vector2 CalculateHidePosition()
        {
            float panelHeight = _goRectTransform.rect.height;
            
            Vector2 hidePosition = _panelStartPosition;

            hidePosition.y = _panelStartPosition.y - panelHeight;

            return hidePosition;
        }
    }
}