using System.Collections.Generic;
using App.Scripts.Resources;
using UnityEditor.Build;
using UnityEngine;

namespace App.Scripts.Buildings
{
    public class BuildSystem : MonoBehaviour
    {
        private readonly ResourceManager _resourceManager;

        public BuildSystem(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public bool CanAccommodateBuilding(BasicBuildingConfig placingObjectConfig)
        {
            List<ResourceRequirement> resourcesToBuild = placingObjectConfig.resourcesToBuild;

            foreach (var resource in resourcesToBuild)
            {
                var definiteResource = _resourceManager.GetResourceData(resource.resourceType).currentAmount;
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