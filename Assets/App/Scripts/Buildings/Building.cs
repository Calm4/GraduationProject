using UnityEngine;

namespace App.Scripts.Buildings
{
    public class Building : MonoBehaviour
    {
        [field: SerializeField] public BasicBuildingConfig BuildingConfig { get; private set; }
        
        public void Initialize(BasicBuildingConfig config)
        {
            BuildingConfig = config;
        }
    }
}