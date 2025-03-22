using System;
using App.Scripts.UI.Buttons;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Buildings.UI
{
    public class BuildingButtonsUI : MonoBehaviour
    {
        [Header("Префаб кнопки для действий здания")] 
        [SerializeField] private GameObject buildingButtonPrefab;

        [Header("Контейнер для кнопок здания")] 
        [SerializeField] private RectTransform buildingButtonsContainer;

        [Header("Анимация появления/скрытия")] [SerializeField]
        private float showTime = 0.3f;

        [SerializeField] private float hideTime = 0.3f;

        [Inject] private DiContainer _container;

        private bool _isShown = false;
        private float _lastBuildingClickTime = -1f;
        [SerializeField] private float clickDelayAfterSelection = 0.2f;

        private void Start()
        {
            ResetBuildingButtonsContainer();
            buildingButtonsContainer.gameObject.SetActive(false);
        }

        private void ShowBuildingActionButtons(Building building)
        {
            ResetBuildingButtonsContainer();

            foreach (var buttonConfig in building.BuildingConfig.buildingButtons)
            {
                var buttonInstance = _container.InstantiatePrefab(buildingButtonPrefab, buildingButtonsContainer);

                var buttonText = buttonInstance.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    buttonText.text = buttonConfig.buttonTitle;
                }

                var buttonImage = buttonInstance.transform.GetChild(0).GetChild(0).GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.sprite = buttonConfig.buttonIcon;
                }

                var actionButton = buttonInstance.GetComponent<BuildingActionButton>();
                
                if (actionButton != null)
                {
                    actionButton.Initialize(buttonConfig.uiPanelPrefab, building);
                }
            }

            _lastBuildingClickTime = Time.time;
            OpenButtonsPanel();
        }

        private void OpenButtonsPanel()
        {
            buildingButtonsContainer.gameObject.SetActive(true);
            buildingButtonsContainer.localScale = Vector3.zero;
            buildingButtonsContainer.DOScale(Vector3.one, showTime)
                .SetEase(Ease.OutBack)
                .OnComplete(() => _isShown = true);
        }

        private void CloseButtonsPanel()
        {
            buildingButtonsContainer.DOScale(Vector3.zero, hideTime)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    _isShown = false;
                    buildingButtonsContainer.gameObject.SetActive(false);
                    ResetBuildingButtonsContainer();
                });
        }

        private void Update()
        {
            if (_isShown && UnityEngine.Input.GetMouseButtonDown(0))
            {
                // Если прошло недостаточно времени после выбора здания – не скрываем кнопки
                if (Time.time - _lastBuildingClickTime < clickDelayAfterSelection)
                    return;

                // Если клик не по кнопкам – скрываем панель    
                if (!IsPointerOverUIElement(buildingButtonsContainer, UnityEngine.Input.mousePosition))
                {
                    CloseButtonsPanel();
                }
            }
        }

        private bool IsPointerOverUIElement(RectTransform rectTransform, Vector2 pointerPosition)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pointerPosition, null);
        }

        private void ResetBuildingButtonsContainer()
        {
            foreach (Transform child in buildingButtonsContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnEnable() => Building.OnBuildingClicked += ShowBuildingActionButtons;
        private void OnDisable() => Building.OnBuildingClicked -= ShowBuildingActionButtons;
    }
}