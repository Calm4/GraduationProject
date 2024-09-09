using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Buildings.UI
{
    public class BuildingTypeVisibilityManager : MonoBehaviour
    {
        [SerializeField] private Transform parentContainer;      // Родительский контейнер для контейнеров зданий
        [SerializeField] private Transform buttonsContainer;     // Родительский контейнер для кнопок
        private List<Transform> buildingTypeContainers;          // Автоматический список контейнеров зданий
        private List<Button> buildingTypeButtons;                // Автоматический список кнопок

        private Button _activeButton;
        [SerializeField] private Color defaultColor; 
        [SerializeField] private Color selectedColor;

        private void Start()
        {
            // Автоматически заполняем список контейнеров из дочерних объектов родительского контейнера
            InitializeBuildingTypeContainers();

            // Автоматически заполняем список кнопок из дочерних объектов
            InitializeBuildingTypeButtons();

            // Устанавливаем цвет первой кнопки
            SetButtonColor(buildingTypeButtons[0]);

            // Назначаем обработчики для каждой кнопки
            for (int i = 0; i < buildingTypeButtons.Count; i++)
            {
                int index = i;  // Локальная переменная, чтобы избежать проблем с замыканием
                buildingTypeButtons[i].onClick.AddListener(() => OnBuildingTypeButtonClicked(index));
            }
        }

        // Инициализация контейнеров на основе дочерних объектов
        private void InitializeBuildingTypeContainers()
        {
            buildingTypeContainers = new List<Transform>();

            // Проходим по всем дочерним объектам и добавляем их в список
            foreach (Transform child in parentContainer)
            {
                buildingTypeContainers.Add(child);
            }
        }

        // Инициализация кнопок на основе всех дочерних объектов с компонентом Button
        private void InitializeBuildingTypeButtons()
        {
            buildingTypeButtons = new List<Button>();

            // Находим все кнопки внутри родительского контейнера кнопок
            buildingTypeButtons.AddRange(buttonsContainer.GetComponentsInChildren<Button>());
        }

        private void OnBuildingTypeButtonClicked(int buttonIndex)
        {
            var selectedType = (BuildingType)buttonIndex;  // Предполагается, что индекс кнопки совпадает с индексом BuildingType

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
