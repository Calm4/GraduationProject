using System.Collections.Generic;
using App.Scripts.Resources;

namespace App.Scripts
{
    public class ResourceManager
    {
        private Dictionary<ResourceType, ResourcesData> _resources;

        public ResourceManager(Dictionary<ResourceType,ResourcesData> resources)
        {
            _resources = resources;
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