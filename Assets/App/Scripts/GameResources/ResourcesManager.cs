using System;
using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.GameResources.Money;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.GameResources
{
    public class ResourcesManager : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<ResourceType, ResourceData> _resources;

        public event Action OnUpdateMaterialResources;
        public event Action OnUpdateFinanceResources;

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

        public Dictionary<ResourceType, ResourceData> GetAllResources()
        {
            return _resources;
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
                var resourceData = _resources[resource.config.resourceType];
                resourceData.currentAmount += resource.amount / 2;

                if (resourceData.currentAmount > resourceData.maxAmount)
                {
                    resourceData.currentAmount = resourceData.maxAmount;
                }

                _resources[resource.config.resourceType] = resourceData;
            }

            OnUpdateMaterialResources?.Invoke();
            OnUpdateFinanceResources?.Invoke();
        }

        public void TakeAwayResourcesForConstruction(BasicBuildingConfig placingObjectConfig)
        {
            List<ResourceRequirement> resourcesToBuild = placingObjectConfig.resourcesToBuild;

            List<ResourceRequirement> materialResourcesToBuild = new List<ResourceRequirement>();
            bool isMaterialResourcesUpdated = InitializeSpecificTypeOfResource<MaterialResourceConfig>(resourcesToBuild, materialResourcesToBuild);
            if (isMaterialResourcesUpdated)
                OnUpdateMaterialResources?.Invoke();
            
            
            List<ResourceRequirement> financeResourcesToBuild = new List<ResourceRequirement>();
            bool financeUpdated = InitializeSpecificTypeOfResource<FinanceResourceConfig>(resourcesToBuild, financeResourcesToBuild);

            if (financeUpdated)
                OnUpdateFinanceResources?.Invoke();
            
        }

        private bool InitializeSpecificTypeOfResource<T>(List<ResourceRequirement> resourcesToBuild,
            List<ResourceRequirement> specificResource) where T : BasicResourceConfig
        {
            foreach (var resource in resourcesToBuild)
            {
                if (resource.config is T)
                {
                    specificResource.Add(resource);
                }
            }

            if (specificResource.Count == 0) return false;

            ReduceResource(specificResource);
            return true;
        }

        private void ReduceResource(List<ResourceRequirement> resourcesToBuild)
        {
            foreach (var resource in resourcesToBuild)
            {
                var resourceType = resource.config.resourceType;
                if (_resources.ContainsKey(resourceType))
                {
                    var resourceData = _resources[resourceType];
                    resourceData.currentAmount -= resource.amount;
                    if (resourceData.currentAmount < 0)
                    {
                        resourceData.currentAmount = 0;
                    }

                    _resources[resourceType] = resourceData;
                }
                else
                {
                    Debug.LogError("This type of resource doesn't exists on resources list");
                }
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

            return -1;
        }

        public int GetResourceMaxAmount(ResourceType resourceType)
        {
            if (_resources.TryGetValue(resourceType, out var resource))
            {
                return resource.maxAmount;
            }

            return -1;
        }

        public bool CalculatePossibilityOfPlacingBuilding(BasicBuildingConfig placingObjectConfig)
        {
            List<ResourceRequirement> resourcesToBuild = placingObjectConfig.resourcesToBuild;

            foreach (var resource in resourcesToBuild)
            {
                var definiteResource = GetResourceData(resource.config.resourceType).currentAmount;
                if (definiteResource < resource.amount)
                {
                    Debug.Log($"Здание не может быть построено. Не хватает ресурса {resource.config.resourceType}");
                    return false;
                }
            }

            return true;
        }
    }
}