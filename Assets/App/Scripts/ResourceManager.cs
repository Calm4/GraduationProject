using System;
using System.Collections.Generic;
using App.Scripts.Resources;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts
{
    public class ResourceManager : SerializedMonoBehaviour
    {
        [SerializeField] private List<SpecificResourceConfig> resourceConfigs;
        [OdinSerialize] private Dictionary<ResourcesTypes, ResourcesData> resources = new();

        private void Start()
        {
            foreach (var currentConfig in resourceConfigs)
            {
                resources[currentConfig.resourceType] = new ResourcesData()
                {
                    resourceType = currentConfig.resourceType,
                    currentAmount = 0,
                    maxAmount = currentConfig.initialMaxAmount,
                };
            }
        }
    }
}