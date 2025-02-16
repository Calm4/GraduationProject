using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts
{
    public class BuildingActionButton : MonoBehaviour
    {
        private RectTransform _uiPanelPrefab; 
        private BuildingButtonsUIPanel _buildingButtonsUIPanel;
        private Button _thisButton;
        
        
        
        [Inject]
        public void Construct(BuildingButtonsUIPanel buildingButtonsUIPanel)
        {
            _buildingButtonsUIPanel = buildingButtonsUIPanel;
        }

        private void Awake()
        {
            _thisButton = gameObject.GetComponent<Button>();
            _thisButton.onClick.AddListener(OnButtonClicked);
        }
        
        /// <summary>
        /// Инициализация кнопки данными из BuildingButtonSO.
        /// </summary>
        public void Initialize(RectTransform panelPrefab)
        {
            _uiPanelPrefab = panelPrefab;
        }
        
        private void OnButtonClicked()
        {
            if (_uiPanelPrefab != null && _buildingButtonsUIPanel != null)
            {
                RectTransform instance = Instantiate(_uiPanelPrefab, _buildingButtonsUIPanel.WindowsContainer);
                instance.anchoredPosition = Vector2.zero; // Опционально: сброс позиции
            }
            else
            {
                Debug.LogError("UI Panel Prefab или BuildingButtonsUIPanel не назначены!");
            }
        }
    }
}