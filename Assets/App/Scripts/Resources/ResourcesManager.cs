using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Resources
{
    public class ResourcesManager : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<ResourceType, ResourceData> _resources;

        public ResourcesManager(Dictionary<ResourceType, ResourceData> resources)
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

        public void ReturnHalfOfResourcesForDestructionBuilding(BasicBuildingConfig placedObjectConfig)
        {
            List<ResourceRequirement> returnedResources = placedObjectConfig.resourcesToBuild;

            foreach (var resource in returnedResources)
            {
                var resourceData = _resources[resource.resourceType];
                resourceData.currentAmount += resource.amountToBuild / 2;

                if (resourceData.currentAmount > resourceData.maxAmount)
                {
                    resourceData.currentAmount = resourceData.maxAmount;
                }

                _resources[resource.resourceType] = resourceData;
            }
        }
        public void TakeAwayResourcesForConstruction(BasicBuildingConfig placingObjectConfig)
        {
            List<ResourceRequirement> resourcesToBuild = placingObjectConfig.resourcesToBuild;

            foreach (var resource in resourcesToBuild)
            {
                var resourceData = _resources[resource.resourceType];
                resourceData.currentAmount -= resource.amountToBuild;

                if (resourceData.currentAmount < 0)
                {
                    resourceData.currentAmount = 0;
                }

                _resources[resource.resourceType] = resourceData; 
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
            if (_resources.TryGetValue(resourceType, out var resource))
            {
                return resource.currentAmount;
            }

            return 0;
        }

        public int GetResourceMaxAmount(ResourceType resourceType)
        {
            if (_resources.TryGetValue(resourceType, out var resource))
            {
                return resource.maxAmount;
            }

            return 0;
        }
        public bool CalculatePossibilityOfPlacingBuilding(BasicBuildingConfig placingObjectConfig)
        {
            List<ResourceRequirement> resourcesToBuild = placingObjectConfig.resourcesToBuild;

            foreach (var resource in resourcesToBuild)
            {
                var definiteResource = GetResourceData(resource.resourceType).currentAmount;
                if (definiteResource < resource.amountToBuild)
                {
                    Debug.Log($"Здание не может быть построено. Не хватает ресурса {resource.resourceType}");
                    return false;
                }
            }

            return true;
        }
    }
}