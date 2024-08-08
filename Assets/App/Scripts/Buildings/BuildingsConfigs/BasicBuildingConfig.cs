using System.Collections.Generic;
using App.Scripts.Resources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Buildings.BuildingsConfigs
{
    public class BasicBuildingConfig : SerializedScriptableObject
    {
        [Title("Building Config")]
        public int ID;
        public string buildingName;
        [ReadOnly, ShowInInspector] public BuildingType buildingType;
        
        [Title("Building Parameters")]
        public Mesh mesh;
        public Material material;
        public Vector2Int size;
        
        [Title("Resources")]
        public List<ResourceRequirement> resourcesToBuild;
        public int moneyToBuild;

        [Title("Icon")]
        public Sprite sprite;
    }
}
