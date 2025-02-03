using System.Collections.Generic;
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

        [Title("Building Parameters")]
        public Vector2Int size;
        public Color buildingAssociateColor;
        
        [Title("Resources")]
        public List<ResourceRequirement> maintenanceResources;
        public List<ResourceRequirement> resourcesToBuild;
        public List<ResourceRequirement> incomingResources;
        
        [Title("Start Modifiers")]
        [ListDrawerSettings(Expanded = true)]
        public List<BaseModifierSO> initialModifiers;
        
        [Title("Icon")]
        public Sprite sprite;
    }
}