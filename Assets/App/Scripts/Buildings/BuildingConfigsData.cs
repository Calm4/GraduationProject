using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using UnityEngine;

namespace App.Scripts.Placement.LevelCreatingWindow
{
    [CreateAssetMenu(fileName = "BuildingConfigsData", menuName = "ScriptableObjects/BuildingConfigsData", order = 2)]
    public class BuildingConfigsData : ScriptableObject
    {
        public List<BasicBuildingConfig> buildingConfigs;
    }

}