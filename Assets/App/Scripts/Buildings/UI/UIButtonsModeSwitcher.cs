using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Buildings.UI
{
    public class UIButtonsModeSwitcher : MonoBehaviour
    {
        [SerializeField] private Button buildModeButton;
        [SerializeField] private Button destroyModeButton;

        [SerializeField] private Color selectedButtonColor;
        [SerializeField] private Color unselectedButtonColor;

        private Button _currentlySelectedButton;

        void Start()
        {
            ResetButtonColors();

            buildModeButton.onClick.AddListener(() => OnButtonClick(buildModeButton, destroyModeButton));
            destroyModeButton.onClick.AddListener(() => OnButtonClick(destroyModeButton, buildModeButton));
        }

        private void OnButtonClick(Button clickedButton, Button otherButton)
        {
            if (_currentlySelectedButton == clickedButton)
            {
                _currentlySelectedButton = null;
                ResetButtonColors();
            }
            else
            {
                _currentlySelectedButton = clickedButton;
                SetButtonColors(clickedButton, otherButton);
            }
        }

        private void SetButtonColors(Button selectedButton, Button unselectedButton)
        {
            ColorBlock selectedColorBlock = selectedButton.colors;
            selectedColorBlock.normalColor = selectedButtonColor;
            selectedColorBlock.highlightedColor = selectedButtonColor * 1.2f; 
            selectedColorBlock.pressedColor = selectedButtonColor * 0.8f; 
            selectedColorBlock.selectedColor = selectedButtonColor;
            selectedColorBlock.disabledColor = selectedButtonColor;
            selectedButton.colors = selectedColorBlock;

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
