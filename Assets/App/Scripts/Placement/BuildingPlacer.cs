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

        public void InstantiateAndPlaceBuilding(Building building, GridManager gridManager, Vector3Int gridPosition,
            Transform parentTransform)
        {
            var gridSize = gridManager.GridData.GridSize;
            var gridOffset = new Vector3((float)gridSize.x / 2, 0, (float)gridSize.y / 2);

            var buildingPosition = new Vector3(gridPosition.x - gridOffset.x, 0, gridPosition.z - gridOffset.z);
            
            Building jsonBuilding = Object.Instantiate(building, buildingPosition, Quaternion.identity);
            //???
            jsonBuilding.transform.SetParent(parentTransform);
            
            _gridData.AddObjectAt(jsonBuilding, gridPosition);
        }
    }
}