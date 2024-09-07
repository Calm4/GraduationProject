using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Scripts.Buildings.BuildingsConfigs;

namespace App.Scripts
{
    public class BuildingTypeVisibilityManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> buildingTypeContainers;
        [SerializeField] private List<Button> buildingTypeButtons;

        private Button _activeButton;
        [SerializeField] private Color defaultColor; 
        [SerializeField] private Color selectedColor;

        private void Start()
        {
            SetButtonColor(buildingTypeButtons[0]);
            
            // Назначаем обработчики для каждой кнопки
            for (int i = 0; i < buildingTypeButtons.Count; i++)
            {
                int index = i;  // Локальная переменная, чтобы избежать проблем с замыканием
                buildingTypeButtons[i].onClick.AddListener(() => OnBuildingTypeButtonClicked(index));
            }
        }

        private void OnBuildingTypeButtonClicked(int buttonIndex)
        {
            var selectedType = (BuildingType)buttonIndex;  // Здесь предполагается, что индекс кнопки совпадает с индексом BuildingType

            // Показываем здания выбранного типа
            ShowBuildingType(selectedType);

            // Меняем цвет кнопки
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

        // Меняем цвет на зелёный для активной кнопки, остальные возвращаем в исходный цвет
        private void SetButtonColor(Button clickedButton)
        {
            if (_activeButton != null)
            {
                // Возвращаем предыдущей кнопке исходный цвет
                var prevButtonImage = _activeButton.GetComponent<Image>();
                if (prevButtonImage != null)
                {
                    prevButtonImage.color = defaultColor;
                }
            }

            // Устанавливаем цвет текущей кнопки на зелёный
            var clickedButtonImage = clickedButton.GetComponent<Image>();
            if (clickedButtonImage != null)
            {
                clickedButtonImage.color = selectedColor;
            }

            // Сохраняем ссылку на активную кнопку
            _activeButton = clickedButton;
        }
    }
}
