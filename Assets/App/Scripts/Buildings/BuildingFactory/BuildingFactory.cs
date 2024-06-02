using UnityEngine;

namespace App.Scripts.Buildings.BuildingFactory
{
    public class BuildingFactory
    {
        private readonly Building _buildingPrefab;

        public BuildingFactory(Building buildingPrefab)
        {
            _buildingPrefab = buildingPrefab;
        }

        public Building CreateBuilding(BasicBuildingConfig config)
        {
            Building building = GameObject.Instantiate(_buildingPrefab);
            building.Initialize(config);

            var meshFilter = building.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                meshFilter.mesh = config.mesh;
            }
            var meshRenderer = building.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = config.material;
            }

            return building;
        }
    }
}