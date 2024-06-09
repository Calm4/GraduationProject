using UnityEngine;

namespace App.Scripts.Buildings.BuildingsConfigs
{
    [CreateAssetMenu(fileName = "_DefensiveBuilding", menuName = "Configs/Buildings/DefensiveBuilding", order = 0)]
    public class DefensiveBuildingConfig : BasicBuildingConfig
    {
        private void OnEnable()
        {
            buildingType = BuildingType.Defensive;
        }
    }
}