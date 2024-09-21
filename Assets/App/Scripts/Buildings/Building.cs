using App.Scripts.Buildings.BuildingsConfigs;
using UnityEngine;

namespace App.Scripts.Buildings
{
    public class Building : MonoBehaviour
    {
        [field: SerializeField] public BasicBuildingConfig BuildingConfig { get; private set; }
        /*[field: SerializeField] public Vector2 GridPosition { get; private set; }*/
        
        public void Initialize(BasicBuildingConfig config/*, Vector2 gridPosition*/)
        {
            BuildingConfig = config;
            /*GridPosition = gridPosition;*/
        }
    }
}