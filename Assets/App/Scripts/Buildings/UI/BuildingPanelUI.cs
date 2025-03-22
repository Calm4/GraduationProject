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
        [SerializeField] private EnumDirections enumDirections = EnumDirections.Left;
        
        [Space, Title("Panel Offset")]
        [SerializeField] private int panelXOffset;
        [SerializeField] private int panelYOffset;
        
        private RectTransform _goRectTransform;
        private Vector2 _panelShowPosition;
        private Vector2 _panelHidePosition;
        
        private bool _isShown = false;

        [SerializeField] private Button showAndHideButton;
        [SerializeField] private TMP_Text nameTextField; 
        [SerializeField] private TMP_Text descriptionTextField; 
        [SerializeField] private TMP_Text modifiersTextField; 


        private float _lastBuildingClickTime = -1f;
        [SerializeField] private float clickDelayAfterSelection = 0.2f;

        private void Start()
        {
            _goRectTransform = GetComponent<RectTransform>();
    
            _panelHidePosition = _goRectTransform.anchoredPosition;
    
            _panelShowPosition = CalculateShowPosition(_panelHidePosition);
    
            _isShown = false;
            showAndHideButton.gameObject.SetActive(false);
        }


        private void OnEnable()
        {
            showAndHideButton.onClick.AddListener(TogglePanelVisibility);
            Building.OnBuildingClicked += HandleBuildingClicked;
        }

        private void OnDisable()
        {
            showAndHideButton.onClick.RemoveListener(TogglePanelVisibility);
            Building.OnBuildingClicked -= HandleBuildingClicked;
        }
        
        private void HandleBuildingClicked(Building building)
        {
            UpdateUI(building);
            showAndHideButton.gameObject.SetActive(true);
            _lastBuildingClickTime = Time.time;

            if (!_isShown)
            {
                _goRectTransform.DOAnchorPos(_panelShowPosition, animationsConfig.panelShowTime)
                                  .SetEase(Ease.InOutSine)
                                  .OnComplete(() => { _isShown = true; });
            }
        }
        
        private void TogglePanelVisibility()
        {
            if (_isShown)
            {
                _goRectTransform.DOAnchorPos(_panelHidePosition, animationsConfig.panelHideTime)
                                  .SetEase(DG.Tweening.Ease.InOutSine)
                                  .OnComplete(() => { _isShown = false; });
            }
            else
            {
                _goRectTransform.DOAnchorPos(_panelShowPosition, animationsConfig.panelShowTime)
                                  .SetEase(DG.Tweening.Ease.InOutSine)
                                  .OnComplete(() => { _isShown = true; });
            }
            _placementManager.StopPlacement();
        }


        private void Update()
        {
            if (_isShown && UnityEngine.Input.GetMouseButtonDown(0))
            {
                // Если прошло недостаточно времени после выбора здания – не скрываем панель
                if (Time.time - _lastBuildingClickTime < clickDelayAfterSelection)
                    return;

                // Если клик не по панели и не по кнопке – закрываем панель
                if (!IsPointerOverUIElement(_goRectTransform, UnityEngine.Input.mousePosition) &&
                    !IsPointerOverUIElement(showAndHideButton.GetComponent<RectTransform>(), UnityEngine.Input.mousePosition))
                {
                    ClosePanel();
                }
            }
        }

        private void ClosePanel()
        {
            _goRectTransform.DOAnchorPos(_panelHidePosition, animationsConfig.panelHideTime)
                              .SetEase(DG.Tweening.Ease.InOutSine)
                              .OnComplete(() =>
                              {
                                  _isShown = false;
                                  ClearUI();
                                  showAndHideButton.gameObject.SetActive(false);
                              });
        }
        
        private void UpdateUI(Building building)
        {
            nameTextField.text = building.BuildingConfig.buildingName;
            descriptionTextField.text = building.BuildingConfig.buildingDescription;
            modifiersTextField.text = string.Join("\n", building.StandardModifiers.Select(m => m.Value.ModifierData.Config.modifierName));
        }
        private void ClearUI()
        {
            nameTextField.text = "";
            descriptionTextField.text = "";
            modifiersTextField.text = "";
        }
        
        private bool IsPointerOverUIElement(RectTransform rectTransform, Vector2 pointerPosition)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pointerPosition, null);
        }
        
        private Vector2 CalculateShowPosition(Vector2 hidePosition)
        {
            float panelWidth = _goRectTransform.rect.width;
            float panelHeight = _goRectTransform.rect.height;
            Vector2 showPosition = hidePosition;

            switch (enumDirections)
            {
                case EnumDirections.Up:
                    showPosition.y = hidePosition.y - panelHeight + panelYOffset;
                    break;
                case EnumDirections.Down:
                    showPosition.y = hidePosition.y + panelHeight - panelYOffset;
                    break;
                case EnumDirections.Left:
                    showPosition.x = hidePosition.x - panelWidth + panelXOffset;
                    break;
                case EnumDirections.Right:
                    showPosition.x = hidePosition.x + panelWidth - panelXOffset;
                    break;
            }

            return showPosition;
        }
    }
}
