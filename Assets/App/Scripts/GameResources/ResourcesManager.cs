﻿using System;
using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.GameResources
{
    public class ResourcesManager : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<ResourceType, ResourceData> _resources;
        
        public event Action OnUpdateResources;
        
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
            OnUpdateResources?.Invoke();
        }
        public void TakeAwayResourcesForConstruction(BasicBuildingConfig placingObjectConfig)
        {
            List<ResourceRequirement> resourcesToBuild = placingObjectConfig.resourcesToBuild;

            foreach (var resource in resourcesToBuild)
            {
                var resourceData = _resources[resource.config.resourceType];
                resourceData.currentAmount -= resource.amount;

                if (resourceData.currentAmount < 0)
                {
                    resourceData.currentAmount = 0;
                }

                _resources[resource.config.resourceType] = resourceData; 
            } 
            OnUpdateResources?.Invoke();
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