using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Placement;
using App.Scripts.TurnsBasedSystem;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Buildings.UI
{
    public class BuildingUIController : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Building buildingPrefab;
        
        [SerializeField] private PlacementManager placementManager;
        [SerializeField] private TurnsBasedManager turnsBasedManager;
        
        [Title("Building Configs by Section")] [SerializeField]
        private BuildingsDataBaseBySectionsSO buildingsDataBaseBySections;

        [SerializeField] private List<Transform> buttonsTypeContainers;

        private bool _isActive = true;

        private void Start()
        {
            GenerateButtons();

            turnsBasedManager.OnGamePhaseChange += GameChanges;
        }

        private void GameChanges(GamePhases gamePhase)
        {
            _isActive = !_isActive;
            gameObject.SetActive(_isActive);
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
                    Debug.Log($"No container found for BuildingType: {buildingType}");
                    return;
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