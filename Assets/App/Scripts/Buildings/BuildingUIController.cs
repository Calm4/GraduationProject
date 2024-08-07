using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Placement;
using App.Scripts.Placement.Placement;
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
        [SerializeField] private BasicBuildingConfig[] buildingConfigs;



        private void Start()
        {
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            foreach (var config in buildingConfigs)
            {
                var buttonInstance = Instantiate(buttonPrefab, buttonContainer);
                var button = buttonInstance.GetComponent<Button>();
                var buttonText = buttonInstance.GetComponentInChildren<TMP_Text>(); 

                if (button != null)
                {
                    var tempConfig = config;
                    button.onClick.AddListener(() => OnBuildingButtonClicked(tempConfig));
                    
                    if (buttonText != null)
                    {
                        buttonText.text = config.buildingName;
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