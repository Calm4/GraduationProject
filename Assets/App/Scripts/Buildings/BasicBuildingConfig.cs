using System.Collections.Generic;
using App.Scripts.Resources;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Buildings
{
    public class BasicBuildingConfig : SerializedScriptableObject
    {
        [ReadOnly, ShowInInspector] public BuildingType buildingType;
        public int ID;
        public Mesh mesh;
        public Material material;
        public Vector2Int size;
        public List<ResourceRequirement> resourcesToBuild;
        public int moneyToBuild;
    }
}