using System;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Placement;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.Scripts.Buildings
{
    public class BuildingUIController : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private PlacementManager placementManager;
        [SerializeField] private Building buildingPrefab;

        [Title("Building Configs List")] [SerializeField]
        private BuildingsDataBase buildingsDataBase;

        [SerializeField] private GameObject buttonsPanel;
        [SerializeField] private Transform positionToHidePanel;
        
        private Vector3 _panelStartPosition;
        private bool _isHide = false;

        private void Start()
        {
            _panelStartPosition = buttonsPanel.transform.position; Debug.Log(buttonsPanel.transform.position);
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            foreach (var config in buildingsDataBase.buildingConfigs)
            {
                var buttonInstance = Instantiate(buttonPrefab, buttonContainer);
                var button = buttonInstance.GetComponent<Button>();
                var buttonText = buttonInstance.GetComponentInChildren<TMP_Text>();

                var buttonImage = buttonInstance.transform.GetChild(0).GetComponent<Image>();

                if (button != null)
                {
                    var tempConfig = config;
                    button.onClick.AddListener(() => OnBuildingButtonClicked(tempConfig));

                    if (buttonText != null)
                    {
                        buttonText.text = config.buildingName;
                    }

                    if (buttonImage != null)
                    {
                        buttonImage.sprite = config.sprite;
                    }
                }
            }
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F))
            {
                ShowOrHideBuildingsPanel();
            }
        }

        private void OnBuildingButtonClicked(BasicBuildingConfig buildingConfig)
        {
            placementManager.StartPlacement(buildingConfig);
        }

        public void ShowOrHideBuildingsPanel()
        {
            if (_isHide)
            {
                buttonsPanel.transform.DOMove(_panelStartPosition, 2f);
                Debug.Log(_panelStartPosition);
            }
            else
            {
                buttonsPanel.transform.DOMove(positionToHidePanel.position, 2f);
                Debug.Log(positionToHidePanel.position + "!!!");
            }
            _isHide = !_isHide;
        }
    }
}