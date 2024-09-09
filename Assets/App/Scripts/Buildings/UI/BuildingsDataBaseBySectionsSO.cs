using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Buildings.UI
{
    [CreateAssetMenu(fileName = "BuildingsDataBaseBySections", menuName = "Configs/DataBases/BuildingsDataBaseBySections", order = 0)]
    public class BuildingsDataBaseBySectionsSO : SerializedScriptableObject
    {
        [OdinSerialize] public Dictionary<BuildingType, List<BasicBuildingConfig>> BuildingsDataBaseBySections;
        
    }
}