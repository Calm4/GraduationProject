using App.Scripts.Animations;
using App.Scripts.Placement;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Buildings.UI
{
    public class BuildingPanelUI : MonoBehaviour
    {
        [SerializeField] private RectTransform buttonImage;
        [SerializeField] private AnimationsConfig animationsConfig;
        [FormerlySerializedAs("hideDirection")] [Space][Title("Panel Movement Settings")] [SerializeField] private EnumDirections enumDirections = EnumDirections.Down;

        [Space][Title("Panel Offset")]
        [SerializeField] private int panelXOffset;
        [SerializeField] private int panelYOffset;
        private RectTransform _goRectTransform;
        private bool _isHide;
        private Vector2 _panelStartPosition;

        [SerializeField] private PlacementManager placementManager;
        
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

            //TODO НЕ ЗАБЫТЬ ПРО ЭТУ ГАДОСТЬ
            //placementManager.StopPlacement();

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

            switch (enumDirections)
            {
                case EnumDirections.Up:
                    hidePosition.y = _panelStartPosition.y + panelHeight - panelYOffset;
                    break;
                case EnumDirections.Down:
                    hidePosition.y = _panelStartPosition.y - panelHeight + panelYOffset;
                    break;
                case EnumDirections.Left:
                    hidePosition.x = _panelStartPosition.x - panelWidth + panelXOffset;
                    break;
                case EnumDirections.Right:
                    hidePosition.x = _panelStartPosition.x + panelWidth + panelXOffset;
                    break;
            }

            return hidePosition;
        }
    }
}