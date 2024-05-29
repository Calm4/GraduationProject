using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Buildings
{
    [CreateAssetMenu(fileName = "_BuildingConfig", menuName = "Configs/BuildingConfig", order = 0)]
    public class BuildingConfig : ScriptableObject
    {
        public int ID;
        public Mesh mesh;
        public Material material;
        public Vector2Int size;
        public BuildingType buildingType;
    }
}