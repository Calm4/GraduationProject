using System;
using System.Collections.Generic;
using App.Scripts.Buildings;
using App.Scripts.UI.Buttons;
using App.Scripts.UI.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts
{
    public class BuildingInfoUIWindow : MonoBehaviour, IBuildingButtonInitializer
    {
        [Inject] private OpenPanelsManager _openPanelsManager;
        
        [Header("Header Elements")]
        [SerializeField] private Image buildingIcon;
        [SerializeField] private TMP_Text buildingTitleTextField;
        [SerializeField] private TMP_Text buildingLevelTextField;
        [SerializeField] private Button closeWindowButton;

        [Header("Standard Modifiers Elements")]
        //RectTransform --> CustomScript
        [SerializeField] private List<ModifierBuffUIInfo> standardModifiersPanels;

        //RectTransform --> CustomScript
        [SerializeField] private List<CustomModifierPanelInInfoWindow> customModifiersPanels;
        
        [SerializeField] private Button aboutBuildingButton;
        [SerializeField] private Button upgradeBuildingButton;

        private Building _parentBuilding;
        
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
            Destroy(gameObject);
        }

        public void BaseInitializer(Building parentBuilding)
        {
            _parentBuilding = parentBuilding;
        }
    }
}