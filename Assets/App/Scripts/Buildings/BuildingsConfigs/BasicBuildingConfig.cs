using System.Collections.Generic;
using App.Scripts.Resources;
using UnityEngine;
using Sirenix.OdinInspector;

namespace App.Scripts.Buildings.BuildingsConfigs
{
    [CreateAssetMenu(fileName = "BasicBuildingConfig", menuName = "Configs/Buildings/BasicBuildingConfig", order = 0)]
    public class BasicBuildingConfig : SerializedScriptableObject
    {
        [Title("Building Config")]
        public int ID;

        public string buildingName;
        public string buildingDescription;
        [ShowInInspector] public BuildingType buildingType;

        [Title("Building Parameters")]
        public Mesh mesh;
        public Material material;
        public Vector2Int size;
        public Color buildingAssociateColor;
        
        [Title("Resources")]
        public List<ResourceRequirement> resourcesToBuild;
        public List<ResourceRequirement> maintenanceResources;

        [Title("Icon")]
        public Sprite sprite;
        
        
    }
}