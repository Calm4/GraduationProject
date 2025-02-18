using System.Collections.Generic;
using App.Scripts.Buildings.UI.BuildingButtons.Configs;
using App.Scripts.GameResources;
using App.Scripts.Modifiers.Configs;
using UnityEngine;
using Sirenix.OdinInspector;

namespace App.Scripts.Buildings.BuildingsConfigs
{
    [CreateAssetMenu(fileName = "BasicBuildingConfig", menuName = "Configs/Gameplay Objects/Buildings/BasicBuildingConfig", order = 0)]
    public class BasicBuildingConfig : SerializedScriptableObject
    {
        [Title("Building Config")]
        public int ID;
        public string buildingName;
        public string buildingDescription;
        public BuildingType buildingType;
        
        [Title("Icon")]
        public Sprite sprite;

        [Title("Building Parameters")]
        public Vector2Int size;
        public Color buildingAssociateColor;
        
        [Title("Resources")]
        public List<ResourceRequirement> maintenanceResources;
        public List<ResourceRequirement> resourcesToBuild;
        public List<ResourceRequirement> incomingResources;
        
        [Title("Start Modifiers")]
        [ListDrawerSettings(ShowFoldout = true)]
        public List<BaseModifierSO> initialModifiers;

        [Title("Building UI Buttons")]
        [ListDrawerSettings(ShowFoldout = true)]
        [Tooltip("Список кнопок, которые появятся при выборе здания")]
        public List<BuildingButtonSO> buildingButtons; 
    }
}