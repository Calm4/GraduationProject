using App.Scripts.Placement;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Buildings.BuildingFactory
{
    public class BuildingUIController : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab; 
        [SerializeField] private Transform buttonContainer; 
        [SerializeField] private PlacementSystem placementSystem;
        [SerializeField] private Building buildingPrefab;
        [Title("Building Configs List")]
        [SerializeField] private BasicBuildingConfig[] buildingConfigs;



        private void Start()
        {
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            for (int i = 0; i < buildingConfigs.Length; i++)
            {
                var config = buildingConfigs[i];
                var buttonInstance = Instantiate(buttonPrefab, buttonContainer);
                var button = buttonInstance.GetComponent<Button>();
                var buttonText = buttonInstance.GetComponentInChildren<TMP_Text>(); 

                if (button != null)
                {
                    button.onClick.AddListener(() => OnBuildingButtonClicked(config));
                    
                    if (buttonText != null)
                    {
                        buttonText.text = config.buildingName;
                    }
                }
            }
        }

        private void OnBuildingButtonClicked(BasicBuildingConfig buildingConfig)
        {
            placementSystem.StartPlacement(buildingConfig);
        }
    }
}