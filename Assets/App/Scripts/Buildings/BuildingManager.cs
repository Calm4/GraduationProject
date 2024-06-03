using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Buildings
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private List<Building> _placedBuildings = new List<Building>();

        public Building CreateBuilding(Building buildingPrefab)
        {
            Building building = Instantiate(buildingPrefab);
            building.Initialize(buildingPrefab.BuildingConfig);

            InitializeComponents(building);

            return building;
        }

        private void InitializeComponents(Building building)
        {
            var meshFilter = building.GetComponent<MeshFilter>();
            Debug.Log(meshFilter);
            if (meshFilter != null)
            {
                Debug.Log(meshFilter.mesh + " до");
                meshFilter.mesh = building.BuildingConfig.mesh;
                Debug.Log(meshFilter.mesh + " после");
            }
            var meshRenderer = building.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                Debug.Log(meshRenderer.material + " до");
                meshRenderer.material = building.BuildingConfig.material;
                Debug.Log(meshRenderer.material + " после");
            }
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