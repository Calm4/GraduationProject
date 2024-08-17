using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts
{
    public class BuildingPanelUI : MonoBehaviour
    {
        private RectTransform _goRectTransform; 
        [SerializeField] private RectTransform positionToHidePanel; 
    
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
                _goRectTransform.DOAnchorPos(_panelStartPosition, animationTime).SetEase(Ease.InOutSine);  
            }
            else
            {
                _goRectTransform.DOAnchorPos(positionToHidePanel.anchoredPosition, animationTime).SetEase(Ease.InCirc);  
            }
            _isHide = !_isHide;
        }
    }
}
