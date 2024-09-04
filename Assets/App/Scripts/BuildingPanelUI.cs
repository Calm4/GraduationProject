using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts
{
    public class BuildingPanelUI : MonoBehaviour
    {
        [SerializeField] private RectTransform buttonImage;
        [SerializeField] private AnimationsConfig animationsConfig;
        [Space][Title("Panel Movement Settings")] [SerializeField] private HideDirection hideDirection = HideDirection.Down;

        [Space][Title("Panel Offset")]
        [SerializeField] private int panelXOffset;
        [SerializeField] private int panelYOffset;
        private RectTransform _goRectTransform;
        private bool _isHide;
        private Vector2 _panelStartPosition;

        private void Start()
        {
            _goRectTransform = GetComponent<RectTransform>();
            _panelStartPosition = _goRectTransform.anchoredPosition;
        }

        public void ShowOrHideBuildingsPanel()
        {
            RotateImage();
            if (_isHide)
            {
                _goRectTransform.DOAnchorPos(_panelStartPosition, animationsConfig.panelShowTime).SetEase(Ease.InOutSine);
            }
            else
            {
                Vector2 hidePosition = CalculateHidePosition();
                _goRectTransform.DOAnchorPos(hidePosition, animationsConfig.panelHideTime).SetEase(Ease.InOutSine);
            }

            _isHide = !_isHide;
        }

        private void RotateImage()
        {
            buttonImage.transform.DORotate(new Vector3(180, 0, 0), animationsConfig.panelImageRotateTime, RotateMode.LocalAxisAdd);
        }

        private Vector2 CalculateHidePosition()
        {
            float panelWidth = _goRectTransform.rect.width;
            float panelHeight = _goRectTransform.rect.height;

            Vector2 hidePosition = _panelStartPosition;

            switch (hideDirection)
            {
                case HideDirection.Up:
                    hidePosition.y = _panelStartPosition.y + panelHeight - panelYOffset;
                    break;
                case HideDirection.Down:
                    hidePosition.y = _panelStartPosition.y - panelHeight + panelYOffset;
                    break;
                case HideDirection.Left:
                    hidePosition.x = _panelStartPosition.x - panelWidth + panelXOffset;
                    break;
                case HideDirection.Right:
                    hidePosition.x = _panelStartPosition.x + panelWidth + panelXOffset;
                    break;
            }

            return hidePosition;
        }
    }
}