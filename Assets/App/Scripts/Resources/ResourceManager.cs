using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Resources
{
    public class ResourceManager : SerializedMonoBehaviour
    {
        [SerializeField] private readonly Dictionary<ResourceType, ResourceData> _resources;

        public ResourceManager(Dictionary<ResourceType,ResourceData> resources)
        {
            _resources = resources;
        }

        public void PassiveIncreaseResources(int amount, ResourceType resourceType, int timeInterval)
        {
            
        }
        
        public ResourceData GetResourceData(ResourceType resourceType)
        {
            return _resources[resourceType];
        }
        
        public void AddResource(int amount, ResourceType resourceType)
        {
            if (_resources.ContainsKey(resourceType))
            {
                var resourceData = _resources[resourceType];
                resourceData.currentAmount += amount;

                if (resourceData.currentAmount > resourceData.maxAmount)
                {
                    resourceData.currentAmount = resourceData.maxAmount;
                }

                _resources[resourceType] = resourceData;
            }
        }

        public void ReduceResource(int amount, ResourceType resourceType)
        {
            if (_resources.ContainsKey(resourceType))
            {
                var resourceData = _resources[resourceType];
                resourceData.currentAmount -= amount;
                if (resourceData.currentAmount < 0)
                {
                    resourceData.currentAmount = 0;
                }

                _resources[resourceType] = resourceData;
            }
        }

        public void IncreaseMaxResourceAmount(int amount, ResourceType resourceType)
        {
            if (_resources.ContainsKey(resourceType))
            {
                var resourceData = _resources[resourceType];
                resourceData.maxAmount += amount;
                _resources[resourceType] = resourceData;
            }
        }

        public int GetResourceCurrentAmount(ResourceType resourceType)
        {
            if (_resources.ContainsKey(resourceType))
            {
                return _resources[resourceType].currentAmount;
            }

            return 0;
        }

        public int GetResourceMaxAmount(ResourceType resourceType)
        {
            if (_resources.ContainsKey(resourceType))
            {
                return _resources[resourceType].maxAmount;
            }

            return 0;
        }
    }
}