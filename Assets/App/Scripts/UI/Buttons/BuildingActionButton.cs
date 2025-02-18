using App.Scripts.Buildings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts
{
    public class BuildingActionButton :  MonoBehaviour
    {
        [Inject] private OpenPanelsManager _openPanelsManager;

        private Building _parentBuilding;
        
        private RectTransform _uiPanelPrefab;
        private BuildingButtonsUIPanel _buildingButtonsUIPanel;
        private Button _thisButton;

        [Inject] private DiContainer _container;

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

        public void Initialize(RectTransform panelPrefab, Building parentBuilding)
        {
            _uiPanelPrefab = panelPrefab;
            _parentBuilding = parentBuilding;
        }

        private void OnButtonClicked()
        {
            if (_uiPanelPrefab != null && _buildingButtonsUIPanel != null)
            {
                CreateCustomWindow(_uiPanelPrefab.gameObject, _buildingButtonsUIPanel.WindowsContainer);
            }
            else
            {
                Debug.LogError("UI Panel Prefab или BuildingButtonsUIPanel не назначены!");
            }
        }

        private void CreateCustomWindow(GameObject uiPanelPrefab, RectTransform windowContainer)
        {
            GameObject instanceGo = _container.InstantiatePrefab(uiPanelPrefab, windowContainer);
            
            RectTransform instance = instanceGo.GetComponent<RectTransform>();
            if (instance.TryGetComponent(out IBuildingButtonInitializer uiBuildingActionButton))
            {
                uiBuildingActionButton.BaseInitializer(_parentBuilding);
                _openPanelsManager.RegisterWindow(_parentBuilding, uiBuildingActionButton);

            }

            instance.anchoredPosition = Vector2.zero;
        }
    }
}