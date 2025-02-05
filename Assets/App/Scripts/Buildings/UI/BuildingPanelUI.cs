using System.Linq;
using App.Scripts.Animations;
using App.Scripts.Placement;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace App.Scripts.Buildings.UI
{
    public class BuildingPanelUI : MonoBehaviour
    {
        [Inject] private PlacementManager _placementManager;
        
        [SerializeField] private AnimationsConfig animationsConfig;
        
        [Space, Title("Panel Movement Settings")]
        [SerializeField] private EnumDirections enumDirections = EnumDirections.Down;

        [Space][Title("Panel Offset")]
        [SerializeField] private int panelXOffset;
        [SerializeField] private int panelYOffset;
        private RectTransform _goRectTransform;
        private bool _isHide;
        private Vector2 _panelStartPosition;

        [Header("UI Elements")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text modifiersText;
        
        private void OnEnable()
        {
            Building.OnBuildingClicked += HandleBuildingClicked;
        }

        private void OnDisable()
        {
            Building.OnBuildingClicked -= HandleBuildingClicked;
        }
        
        private void HandleBuildingClicked(Building building)
        {
            Debug.Log(building.BuildingConfig.buildingName + "!!!666");
            UpdateUI(building);
            ShowPanel();
        }
        
        private void Start()
        {
            _goRectTransform = GetComponent<RectTransform>();
            _panelStartPosition = _goRectTransform.anchoredPosition;
        }
        
        private void UpdateUI(Building building)
        {
            Debug.Log(building.BuildingConfig.buildingName + "------------------------------");
            titleText.text = building.BuildingConfig.buildingName;
            descriptionText.text = building.BuildingConfig.buildingDescription;
            modifiersText.text = GetModifiersText(building);
        }

        private string GetModifiersText(Building building)
        {
            return string.Join("\n", building.ActiveModifiers.Select(m => m.modifierName));
        }
        
        public void ShowPanel()
        {
            if (!_isHide) return;
            _goRectTransform.DOAnchorPos(_panelStartPosition, animationsConfig.panelShowTime)
                .SetEase(Ease.InOutSine);
            _isHide = false;
        }

        public void HidePanel()
        {
            if (_isHide) return;
            Vector2 hidePosition = CalculateHidePosition();
            _goRectTransform.DOAnchorPos(hidePosition, animationsConfig.panelHideTime)
                .SetEase(Ease.InOutSine);
            _isHide = true;
        }
        
        public void ShowOrHideBuildingsPanel()
        {
            if (_isHide) ShowPanel();
            else HidePanel();
        }
        
        /*public void ShowOrHideBuildingsPanel()
        {
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
        }*/

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