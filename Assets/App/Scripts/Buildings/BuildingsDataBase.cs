using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using UnityEngine;

namespace App.Scripts.Buildings
{
    [CreateAssetMenu(fileName = "BuildingConfigsData", menuName = "Configs/BuildingsDataBase", order = 2)]
    public class BuildingsDataBase : ScriptableObject
    {
        public List<BasicBuildingConfig> buildingConfigs;
    }

}