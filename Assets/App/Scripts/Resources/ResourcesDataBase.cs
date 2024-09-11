using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Resources
{
    [CreateAssetMenu(fileName = "ResourcesDataBase", menuName = "Configs/DataBases/ResourcesDataBase", order = 2)]
    public class ResourcesDataBase : ScriptableObject
    {
        public List<BasicResourceConfig> resourcesConfigs;
    }
}
