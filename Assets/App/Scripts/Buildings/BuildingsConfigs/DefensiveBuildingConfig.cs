using App.Scripts.Projectiles;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Buildings.BuildingsConfigs
{
    [CreateAssetMenu(fileName = "_DefensiveBuilding", menuName = "Configs/Gameplay Objects/Buildings/DefensiveBuilding", order = 0)]
    public class DefensiveBuildingConfig : BasicBuildingConfig
    {
        [Title("Projectile")] public Projectile projectilePrefab;
        
        private void OnEnable()
        {
            buildingType = BuildingType.Defensive;
        }
    }
}