using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Buildings
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private List<Building> placedBuildings = new();
        [SerializeField] private Transform buildingsContainer;
        [SerializeField] private Vector3 buildingOffsetPoint;
        [SerializeField] private AnimationsConfig animationsConfig;
        public Building CreateBuilding(Building buildingPrefab)
        {
            Building building = Instantiate(buildingPrefab, buildingsContainer);
            building.Initialize(buildingPrefab.BuildingConfig);

            InitializeComponents(building);

            return building;
        }

        private void InitializeComponents(Building building)
        {
            building.gameObject.name = building.BuildingConfig.name;

            if (building.TryGetComponent(out MeshFilter meshFilter))
            {
                meshFilter.mesh = building.BuildingConfig.mesh;
            }
            if (building.TryGetComponent(out MeshRenderer meshRenderer))
            {
                meshRenderer.material = building.BuildingConfig.material;
            }
            if (building.TryGetComponent(out MeshCollider meshCollider))
            {
                meshCollider.sharedMesh = building.BuildingConfig.mesh;
            }
        }

        public Building PlaceBuilding(Building buildingPrefab, Vector3 position)
        {
            Building building = CreateBuilding(buildingPrefab);
            building.transform.position = position + buildingOffsetPoint;
            building.transform.DOMove(position, animationsConfig.buildingPlacingTime).SetEase(Ease.InCirc);
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
    }
}
