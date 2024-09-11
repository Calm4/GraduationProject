using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Placement;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Buildings.UI
{
    public class BuildingUIController : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private List<Transform> buttonsTypeContainers;
        [SerializeField] private PlacementManager placementManager;
        [SerializeField] private Building buildingPrefab;

        [Title("Building Configs by Section")] [SerializeField]
        private BuildingsDataBaseBySectionsSO buildingsDataBaseBySections;

        [SerializeField] private BuildingsDescriptionUIPanel buildingsDescriptionUIPanel;


        private void Start()
        {
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            foreach (var section in buildingsDataBaseBySections.BuildingsDataBaseBySections)
            {
                var buildingType = section.Key;

                var matchingContainer = buttonsTypeContainers.Find(container =>
                {
                    var typeComponent = container.GetComponent<BuildingTypeComponent>();
                    return typeComponent != null && typeComponent.BuildingType == buildingType;
                });

                if (matchingContainer == null)
                {
                    Debug.LogError($"No container found for BuildingType: {buildingType}");
                    continue;
                }

                foreach (var config in section.Value)
                {
                    var buttonInstance = Instantiate(buttonPrefab, matchingContainer);
                    var button = buttonInstance.GetComponent<Button>();
                    var buttonText = buttonInstance.GetComponentInChildren<TMP_Text>();
                    var buttonImage = buttonInstance.transform.GetChild(0).GetComponent<Image>();
                    var buttonDescription = buttonInstance.GetComponent<BuildingsDescriptionUIPanel>();
                    if (button != null)
                    {
                        var tempConfig = config;
                        button.onClick.AddListener(() => OnBuildingButtonClicked(tempConfig));

                        buttonDescription.Initialize(tempConfig);
                        
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