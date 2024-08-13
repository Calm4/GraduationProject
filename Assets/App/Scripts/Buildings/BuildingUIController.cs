using System;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Placement;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Buildings
{
    public class BuildingUIController : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab; 
        [SerializeField] private Transform buttonContainer; 
        [SerializeField] private PlacementManager placementManager;
        [SerializeField] private Building buildingPrefab;
        [Title("Building Configs List")]
        [SerializeField] private BuildingsDataBase buildingsDataBase;

        [SerializeField] private GameObject hideAndShowBuildings;
        [SerializeField] private Transform positionToMovePanel;


        private void Start()
        {
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            foreach (var config in buildingsDataBase.buildingConfigs)
            {
                var buttonInstance = Instantiate(buttonPrefab, buttonContainer);
                var button = buttonInstance.GetComponent<Button>();
                var buttonText = buttonInstance.GetComponentInChildren<TMP_Text>();

                // Предположим, что Image является первым дочерним элементом Button
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

            hideAndShowBuildings.transform.DOMove(positionToMovePanel.position, 2f);
        }
    }
}