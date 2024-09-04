using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts
{
    public class UIButtonsModeSwitcher : MonoBehaviour
    {
        [SerializeField] private Button buildModeButton;
        [SerializeField] private Button destroyModeButton;

        [SerializeField] private Color selectedButtonColor;
        [SerializeField] private Color unselectedButtonColor;

        private Button _currentlySelectedButton = null;

        void Start()
        {
            // Задаем начальные цвета для кнопок
            ResetButtonColors();

            // Добавляем обработчики событий нажатия на кнопки
            buildModeButton.onClick.AddListener(() => OnButtonClick(buildModeButton, destroyModeButton));
            destroyModeButton.onClick.AddListener(() => OnButtonClick(destroyModeButton, buildModeButton));
        }

        private void OnButtonClick(Button clickedButton, Button otherButton)
        {
            // Проверяем, нажата ли уже выбранная кнопка
            if (_currentlySelectedButton == clickedButton)
            {
                // Сбрасываем выбор, если нажали на ту же кнопку
                _currentlySelectedButton = null;
                ResetButtonColors();
            }
            else
            {
                // Устанавливаем новую выбранную кнопку
                _currentlySelectedButton = clickedButton;
                SetButtonColors(clickedButton, otherButton);
            }
        }

        private void SetButtonColors(Button selectedButton, Button unselectedButton)
        {
            // Настройка всех состояний для выбранной кнопки
            ColorBlock selectedColorBlock = selectedButton.colors;
            selectedColorBlock.normalColor = selectedButtonColor;
            selectedColorBlock.highlightedColor = selectedButtonColor * 1.2f; // немного светлее
            selectedColorBlock.pressedColor = selectedButtonColor * 0.8f; // немного темнее
            selectedColorBlock.selectedColor = selectedButtonColor;
            selectedColorBlock.disabledColor = selectedButtonColor;
            selectedButton.colors = selectedColorBlock;

            // Настройка всех состояний для невыбранной кнопки
            ColorBlock unselectedColorBlock = unselectedButton.colors;
            unselectedColorBlock.normalColor = unselectedButtonColor;
            unselectedColorBlock.highlightedColor = unselectedButtonColor * 1.2f;
            unselectedColorBlock.pressedColor = unselectedButtonColor * 0.8f;
            unselectedColorBlock.selectedColor = unselectedButtonColor;
            unselectedColorBlock.disabledColor = unselectedButtonColor;
            unselectedButton.colors = unselectedColorBlock;
        }

        private void ResetButtonColors()
        {
            // Сброс всех состояний для обеих кнопок в невыбранное состояние
            ColorBlock buildButtonColors = buildModeButton.colors;
            buildButtonColors.normalColor = unselectedButtonColor;
            buildButtonColors.highlightedColor = unselectedButtonColor * 1.2f;
            buildButtonColors.pressedColor = unselectedButtonColor * 0.8f;
            buildButtonColors.selectedColor = unselectedButtonColor;
            buildButtonColors.disabledColor = unselectedButtonColor;
            buildModeButton.colors = buildButtonColors;

            ColorBlock destroyButtonColors = destroyModeButton.colors;
            destroyButtonColors.normalColor = unselectedButtonColor;
            destroyButtonColors.highlightedColor = unselectedButtonColor * 1.2f;
            destroyButtonColors.pressedColor = unselectedButtonColor * 0.8f;
            destroyButtonColors.selectedColor = unselectedButtonColor;
            destroyButtonColors.disabledColor = unselectedButtonColor;
            destroyModeButton.colors = destroyButtonColors;
        }
    }
}
