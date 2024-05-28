using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System;

namespace App.Scripts.Resources
{
    public class ResourcesInitializer : SerializedMonoBehaviour
    {
        [SerializeField] private List<ResourceConfig> resourceConfigs;
        [OdinSerialize] private Dictionary<ResourceType, ResourcesData> resources = new();
        
        private void Start()
        {
            ResourceType[] resourceTypes = (ResourceType[])Enum.GetValues(typeof(ResourceType));
            
            foreach (var currentConfig in resourceConfigs)
            {
                resources[] = new ResourcesData()
                {
                    currentAmount = 0,
                    maxAmount = currentConfig.initialMaxAmount,
                };
            }
        }
    }
}