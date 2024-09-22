using App.Scripts.Buildings.BuildingsConfigs;
using UnityEngine;

namespace App.Scripts.Buildings
{
    public class Building : MonoBehaviour
    {
        [field: SerializeField] public BasicBuildingConfig BuildingConfig { get; private set; }
        
    }
}