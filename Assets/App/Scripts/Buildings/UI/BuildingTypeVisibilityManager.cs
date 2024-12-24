using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Buildings.UI
{
    public class BuildingTypeVisibilityManager : MonoBehaviour
    {
        [SerializeField] private Transform parentContainer;
        [SerializeField] private Transform buttonsContainer;
        private List<Transform> buildingTypeContainers;
        private List<Button> buildingTypeButtons;

        private Button _activeButton;
        [SerializeField] private Color defaultColor; 
        [SerializeField] private Color selectedColor;

        private void Start()
        {
            InitializeBuildingTypeContainers();

            InitializeBuildingTypeButtons();

            SetButtonColor(buildingTypeButtons[0]);

            for (int i = 0; i < buildingTypeButtons.Count; i++)
            {
                int index = i;  
                buildingTypeButtons[i].onClick.AddListener(() => OnBuildingTypeButtonClicked(index));
            }
        }

        private void InitializeBuildingTypeContainers()
        {
            buildingTypeContainers = new List<Transform>();

            foreach (Transform child in parentContainer)
            {
                buildingTypeContainers.Add(child);
            }
        }

        private void InitializeBuildingTypeButtons()
        {
            buildingTypeButtons = new List<Button>();

            buildingTypeButtons.AddRange(buttonsContainer.GetComponentsInChildren<Button>());
        }

        private void OnBuildingTypeButtonClicked(int buttonIndex)
        {
            var selectedType = (BuildingType)buttonIndex;  

            ShowBuildingType(selectedType);

            SetButtonColor(buildingTypeButtons[buttonIndex]);
        }

        public void ShowBuildingType(BuildingType selectedType)
        {
            foreach (var container in buildingTypeContainers)
            {
                var typeComponent = container.GetComponent<BuildingTypeComponent>();
                if (typeComponent != null)
                {
                    container.gameObject.SetActive(typeComponent.BuildingType == selectedType);
                }
            }
        }

        private void SetButtonColor(Button clickedButton)
        {
            if (_activeButton != null)
            {
                var prevButtonImage = _activeButton.GetComponent<Image>();
                if (prevButtonImage != null)
                {
                    prevButtonImage.color = defaultColor;
                }
            }

            var clickedButtonImage = clickedButton.GetComponent<Image>();
            if (clickedButtonImage != null)
            {
                clickedButtonImage.color = selectedColor;
            }

            _activeButton = clickedButton;
        }
    }
}
