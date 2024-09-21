using App.Scripts.Buildings;
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

        public void PlaceBuilding(Building building, GridManager gridManager, Vector3Int position, Transform parentTransform)
        {
            var gridSize = gridManager.GridData.GridSize;
            var gridOffset = new Vector3((float)gridSize.x / 2, 0, (float)gridSize.y / 2);
            var buildingPosition = new Vector3(position.x - gridOffset.x, 0, position.z - gridOffset.z);
            Building tempBuilding = Object.Instantiate(building,buildingPosition, Quaternion.identity);
            tempBuilding.transform.SetParent(parentTransform);

            /*
            var buildingObject = new GameObject(building.BuildingConfig.buildingName)
            {
                transform =
                {
                    position = new Vector3(position.x - gridOffset.x, 0, position.z - gridOffset.z)
                },
            };*/

            
            /*
            Building buildingComponent = buildingObject.AddComponent<Building>();
            buildingComponent.Initialize(building.BuildingConfig);*/

            //TODO: НУЖЕН ЛИ ЭТОТ КОД МНЕ????
            /*MeshFilter meshFilter = buildingObject.AddComponent<MeshFilter>();
            meshFilter.mesh = config.mesh;

            MeshRenderer meshRenderer = buildingObject.AddComponent<MeshRenderer>();
            meshRenderer.material = config.material;

            MeshCollider meshCollider = buildingObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = config.mesh;*/
            
            _gridData.AddObjectAt(position, tempBuilding);
        }
    }
}