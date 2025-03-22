using System;
using App.Scripts.Buildings;
using App.Scripts.UI.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts
{
    public class BuildingInfoHeaderUIPanel : MonoBehaviour
    {
        [Inject] private OpenPanelsManager _openPanelsManager;
        
        [Header("Header Elements")]
        [SerializeField] private BuildingInfoUIWindow buildingInfoUIWindow;
        [SerializeField] private Image buildingIcon;
        [SerializeField] private TMP_Text buildingTitleTextField;
        [SerializeField] private TMP_Text buildingLevelTextField;
        [SerializeField] private Button closeWindowButton;

        private const string LevelTextField = "Level: ";
        private Building _parentBuilding;
        
        public void Initialize(Building parentBuilding)
        {
            _parentBuilding = parentBuilding;
            InitializeHeader(_parentBuilding);
        }

        private void InitializeHeader(Building building)
        {
            if (building == null)
            {
                Debug.LogError("Building is null");
                return;
            }
            if (building.BuildingConfig == null)
            {
                Debug.LogError("BuildingConfig is null");
                return;
            }
            if (buildingIcon == null)
            {
                Debug.LogError("buildingIcon is null");
                return;
            }
    
            buildingIcon.sprite = building.BuildingConfig.sprite;
            buildingTitleTextField.text = building.BuildingConfig.buildingName;
            buildingLevelTextField.text = string.Concat(LevelTextField, building.BuildingConfig.buildingLevel);
        }


        private void Awake()
        {
            closeWindowButton.onClick.AddListener(CloseWindow);
        }



        private void CloseWindow()
        {
            if (_parentBuilding != null)
            {
                _openPanelsManager.UnregisterWindow(_parentBuilding);
            }
            Destroy(buildingInfoUIWindow.gameObject);
        }
    }
}