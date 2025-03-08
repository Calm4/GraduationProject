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
        [Header("General Panels")]
        [SerializeField] private BuildingInfoHeaderUIPanel buildingInfoHeaderUIPanel;
        [SerializeField] private StandardModifierUIPanel standardModifierUIPanel;
        [SerializeField] private CustomModifierUIPanel customModifierUIPanel;
        [SerializeField] private OtherButtonsModifierUIPanel otherButtonsModifierUIPanel;
        
        [Space(10),Header("Variant Buttons")]
        [SerializeField] private Button aboutBuildingButton;
        [SerializeField] private Button upgradeBuildingButton;

        private Building _parentBuilding;
        

        public void BaseInitializer(Building parentBuilding)
        {
            _parentBuilding = parentBuilding;
            
            buildingInfoHeaderUIPanel.Initialize(_parentBuilding);
            standardModifierUIPanel.Initialize(_parentBuilding);
            customModifierUIPanel.Initialize(_parentBuilding);
        }
    }
}