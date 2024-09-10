using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class BuildingPlacer
    {
        private readonly GridData _gridData;

        public BuildingPlacer(GridData gridData)
        {
            _gridData = gridData;
        }

        public void PlaceBuilding(BasicBuildingConfig config, GridManager gridManager, Vector3Int position, Transform parentTransform)
        {
            var gridOffset = new Vector3((float)gridManager.GridSize.x / 2, 0, (float)gridManager.GridSize.y / 2);

            var buildingObject = new GameObject(config.buildingName)
            {
                transform =
                {
                    position = new Vector3(position.x - gridOffset.x, 0, position.z - gridOffset.z)
                },
            };
            buildingObject.transform.SetParent(parentTransform);
            Building buildingComponent = buildingObject.AddComponent<Building>();
            buildingComponent.Initialize(config);

            MeshFilter meshFilter = buildingObject.AddComponent<MeshFilter>();
            meshFilter.mesh = config.mesh;

            MeshRenderer meshRenderer = buildingObject.AddComponent<MeshRenderer>();
            meshRenderer.material = config.material;

            MeshCollider meshCollider = buildingObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = config.mesh;
            
            Vector2Int buildingSize = config.size;
            _gridData.AddObjectAt(position, buildingSize, buildingComponent);
        }
    }
}