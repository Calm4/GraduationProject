using System.Collections.Generic;
using App.Scripts.Placement;
using App.Scripts.TurnsBasedSystem;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Buildings.UI
{
    public class BuildingUIController : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Building buildingPrefab;
        
        [Inject] private PlacementManager _placementManager;
        [Inject] private TurnsBasedManager _turnsBasedManager;
        
        [Title("Building Configs by Section")] [SerializeField]
        private BuildingsDataBaseBySectionsSO buildingsDataBaseBySections;

        [SerializeField] private List<Transform> buttonsTypeContainers;

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
                    Debug.Log($"No container found for BuildingType: {buildingType}");
                    return;
                }

                foreach (var building in section.Value)
                {
                    var buttonInstance = Instantiate(buttonPrefab, matchingContainer);
                    var button = buttonInstance.GetComponent<Button>();
                    var buttonText = buttonInstance.GetComponentInChildren<TMP_Text>();
                    var buttonImage = buttonInstance.transform.GetChild(0).GetComponent<Image>();
                    var buttonDescription = buttonInstance.GetComponent<BuildingsDescriptionUIPanel>();
                    if (button != null)
                    {
                        var tempBuilding = building;
                        button.onClick.AddListener(() => OnBuildingButtonClicked(tempBuilding));

                        buttonDescription.Initialize(tempBuilding);
                        
                        if (buttonText != null)
                        {
                            buttonText.text = building.BuildingConfig.buildingName;
                        }

                        if (buttonImage != null)
                        {
                            buttonImage.sprite = building.BuildingConfig.sprite;
                        }
                    }
                }
            }
        }
        
        private void OnBuildingButtonClicked(Building building)
        {
            _placementManager.StartPlacement(building);
        }
    }
}