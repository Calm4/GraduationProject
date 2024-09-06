using System;
using System.Collections.Generic;
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
        [SerializeField] private List<Transform> buttonsTypeContainers;
        [SerializeField] private PlacementManager placementManager;
        [SerializeField] private Building buildingPrefab;

        [Title("Building Configs by Section")] 
        [SerializeField] private BuildingsDataBaseBySectionsSO buildingsDataBaseBySections;
        
        
        
        private void Start()
        {
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            // Loop through each section (BuildingType and its corresponding building configs)
            foreach (var section in buildingsDataBaseBySections.BuildingsDataBaseBySections)
            {
                var buildingType = section.Key;

                // Find the container that matches the BuildingType
                var matchingContainer = buttonsTypeContainers.Find(container =>
                {
                    var typeComponent = container.GetComponent<BuildingTypeComponent>();
                    return typeComponent != null && typeComponent.BuildingType == buildingType;
                });

                if (matchingContainer == null)
                {
                    Debug.LogError($"No container found for BuildingType: {buildingType}");
                    continue; // Skip if no matching container is found
                }

                // Instantiate buttons for each building config in the matched container
                foreach (var config in section.Value)
                {
                    var buttonInstance = Instantiate(buttonPrefab, matchingContainer); // Instantiate in the correct container
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
        }
        
        private void OnBuildingButtonClicked(BasicBuildingConfig buildingConfig)
        {
            placementManager.StartPlacement(buildingConfig);
        }

        
    }
}
