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
        [SerializeField] private List<ResourceConfig> resourceConfigs;
        [OdinSerialize] private Dictionary<ResourceType, ResourcesData> resources = new();

        private void Start()
        {
            //TODO: перенести инициализацию в отдельный скрипт
            foreach (var currentConfig in resourceConfigs)
            {
                resources[currentConfig.resourceType] = new ResourcesData()
                {
                    currentAmount = 0,
                    maxAmount = currentConfig.initialMaxAmount,
                };
            }
        }

        public void AddResource(int amount, ResourceType resourceType)
        {
            if (resources.ContainsKey(resourceType))
            {
                var resourceData = resources[resourceType];
                resourceData.currentAmount += amount;

                if (resourceData.currentAmount > resourceData.maxAmount)
                {
                    resourceData.currentAmount = resourceData.maxAmount;
                }

                resources[resourceType] = resourceData;
            }
        }

        public void ReduceResource(int amount, ResourceType resourceType)
        {
            if (resources.ContainsKey(resourceType))
            {
                var resourceData = resources[resourceType];
                resourceData.currentAmount -= amount;
                if (resourceData.currentAmount < 0)
                {
                    resourceData.currentAmount = 0;
                }

                resources[resourceType] = resourceData;
            }
        }

        public void IncreaseMaxResourceAmount(int amount, ResourceType resourceType)
        {
            if (resources.ContainsKey(resourceType))
            {
                var resourceData = resources[resourceType];
                resourceData.maxAmount += amount;
                resources[resourceType] = resourceData;
            }
        }

        public int GetResourceCurrentAmount(ResourceType resourceType)
        {
            if (resources.ContainsKey(resourceType))
            {
                return resources[resourceType].currentAmount;
            }

            return 0;
        }

        public int GetResourceMaxAmount(ResourceType resourceType)
        {
            if (resources.ContainsKey(resourceType))
            {
                return resources[resourceType].maxAmount;
            }

            return 0;
        }
    }
}