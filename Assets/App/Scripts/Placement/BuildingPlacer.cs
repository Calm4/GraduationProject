using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class BuildingPlacer
    {
        private readonly GridData _furnitureData;
        private readonly Vector2Int _gridSize;

        public BuildingPlacer(GridData furnitureData, Vector2Int gridSize)
        {
            _furnitureData = furnitureData;
            _gridSize = gridSize;
        }

        public void PlaceBuilding(BasicBuildingConfig config, Vector3Int position)
        {
            var gridOffset = new Vector3((float)_gridSize.x / 2, 0, (float)_gridSize.y / 2);

            var buildingObject = new GameObject(config.buildingName)
            {
                transform =
                {
                    position = new Vector3(position.x - gridOffset.x, 0, position.z - gridOffset.z)
                }
            };

            Building buildingComponent = buildingObject.AddComponent<Building>();
            buildingComponent.Initialize(config);

            MeshFilter meshFilter = buildingObject.AddComponent<MeshFilter>();
            meshFilter.mesh = config.mesh;

            MeshRenderer meshRenderer = buildingObject.AddComponent<MeshRenderer>();
            meshRenderer.material = config.material;

            MeshCollider meshCollider = buildingObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = config.mesh;
            
            Vector2Int buildingSize = config.size;
            _furnitureData.AddObjectAt(position, buildingSize, buildingComponent);
        }
    }
}