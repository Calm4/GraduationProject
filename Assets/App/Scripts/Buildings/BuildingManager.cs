using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Buildings
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private List<Building> _placedBuildings = new List<Building>();

        private Building CreateBuilding(Building buildingPrefab)
        {
            Building building = Instantiate(buildingPrefab);
            building.Initialize(buildingPrefab.BuildingConfig);

            var meshFilter = building.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                meshFilter.mesh = buildingPrefab.BuildingConfig.mesh;
            }
            var meshRenderer = building.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = buildingPrefab.BuildingConfig.material;
            }

            return building;
        }
        public Building PlaceBuilding(Building buildingPrefab, Vector3 position)
        {
            Building building = CreateBuilding(buildingPrefab);
            building.transform.position = position;
            _placedBuildings.Add(building);
            return building;
        }
        public void RemoveBuilding(Building building)
        {
            if (_placedBuildings.Contains(building))
            {
                _placedBuildings.Remove(building);
                Destroy(building.gameObject);
            }
        }

        public int GetIndexOfPlacedBuilding(Building building)
        {
            return _placedBuildings.IndexOf(building);
        }
    }
}