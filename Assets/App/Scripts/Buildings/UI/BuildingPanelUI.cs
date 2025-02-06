using System;
using System.Linq;
using App.Scripts.Animations;
using App.Scripts.Placement;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

        [SerializeField] private Button showAndHideButton;
        [SerializeField] private TMP_Text nameTextField; 
        [SerializeField] private TMP_Text descriptionTextField; 
        [SerializeField] private TMP_Text modifiersTextField; 
        
        private void Start()
        {
            _goRectTransform = GetComponent<RectTransform>();
            _panelStartPosition = _goRectTransform.anchoredPosition;
        }

        private void OnEnable()
        {
            showAndHideButton.onClick.AddListener(ShowOrHideBuildingsPanel);
            Building.OnBuildingClicked += HandleBuildingClicked;
        }

        private void OnDisable()
        {
            showAndHideButton.onClick.RemoveListener(ShowOrHideBuildingsPanel);
            Building.OnBuildingClicked -= HandleBuildingClicked;
        }
        
        private void HandleBuildingClicked(Building building)
        {
            Debug.Log(building.BuildingConfig.buildingName + " IN UI");
            UpdateUI(building);
           // ShowPanel();
        }

        private void UpdateUI(Building building)
        {
            nameTextField.text = building.BuildingConfig.buildingName;
            descriptionTextField.text = building.BuildingConfig.buildingDescription;
            modifiersTextField.text = GetModifiersText(building);
        }
        
        private string GetModifiersText(Building building)
        {
            return string.Join("\n", building.ActiveModifiers.Select(m => m.modifierName));
        }
        
        private void ShowOrHideBuildingsPanel()
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
            _placementManager.StopPlacement();

            _isHide = !_isHide;
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