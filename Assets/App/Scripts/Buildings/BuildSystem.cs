using System.Collections.Generic;
using App.Scripts.Resources;
using UnityEngine;

namespace App.Scripts.Buildings
{
    public class BuildSystem : MonoBehaviour
    {
        private readonly ResourcesManager _resourcesManager;

        public BuildSystem(ResourcesManager resourcesManager)
        {
            _resourcesManager = resourcesManager;
        }

        public bool CanAccommodateBuilding(BasicBuildingConfig placingObjectConfig)
        {
            List<ResourceRequirement> resourcesToBuild = placingObjectConfig.resourcesToBuild;

            foreach (var resource in resourcesToBuild)
            {
                var definiteResource = _resourcesManager.GetResourceData(resource.resourceType).currentAmount;
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