using System.Collections.Generic;
using System.Linq;
using App.Scripts.Buildings.BuildingsConfigs; // Добавляем для использования метода FirstOrDefault
using UnityEngine;

namespace App.Scripts.Buildings
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private List<Building> placedBuildings = new List<Building>();
        [SerializeField] private List<BasicBuildingConfig> availableBuildingConfigs; // Список всех доступных конфигураций зданий

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
            if (meshFilter != null)
            {
                meshFilter.mesh = building.BuildingConfig.mesh;
            }
            var meshRenderer = building.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = building.BuildingConfig.material;
            }
        }

        public Building PlaceBuilding(Building buildingPrefab, Vector3 position)
        {
            Building building = CreateBuilding(buildingPrefab);
            building.transform.position = position;
            placedBuildings.Add(building);
            return building;
        }

        public void RemoveBuilding(Building building)
        {
            if (placedBuildings.Contains(building))
            {
                placedBuildings.Remove(building);
                Destroy(building.gameObject);
            }
        }

        public int GetIndexOfPlacedBuilding(Building building)
        {
            return placedBuildings.IndexOf(building);
        }

        // Новый метод для получения конфигурации здания по его instanceID
        public BasicBuildingConfig GetBuildingConfigById(int instanceID)
        {
            return availableBuildingConfigs.FirstOrDefault(config => config.ID == instanceID);
        }
    }
}
