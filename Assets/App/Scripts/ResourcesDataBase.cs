using System.Collections.Generic;
using App.Scripts.Resources;
using UnityEngine;

namespace App.Scripts
{
    [CreateAssetMenu(fileName = "ResourcesDataBase", menuName = "Configs/DataBases/ResourcesDataBase", order = 2)]
    public class ResourcesDataBase : ScriptableObject
    {
        public List<ResourceConfig> resourcesConfigs;
    }
}
