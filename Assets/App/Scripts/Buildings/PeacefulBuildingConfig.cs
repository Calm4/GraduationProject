using System.Collections.Generic;
using App.Scripts.Resources;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Buildings
{
    [CreateAssetMenu(fileName = "_PeacefulBuilding", menuName = "Configs/Buildings/PeacefulBuilding", order = 0)]
    public class PeacefulBuildingConfig : BasicBuildingConfig
    {
        public List<IncomingResources> incomingResources;
        private void OnEnable()
        {
            buildingType = BuildingType.Peaceful;
        }
    }
}