using System.Collections.Generic;
using App.Scripts.Resources;
using UnityEngine;

namespace App.Scripts.Buildings.BuildingsConfigs
{
    [CreateAssetMenu(fileName = "_PeacefulBuilding", menuName = "Configs/Buildings/PeacefulBuilding", order = 0)]
    public class PeacefulBuildingConfig : BasicBuildingConfig
    { 
        private void OnEnable()
        {
            buildingType = BuildingType.Peaceful;
        }
    }
}