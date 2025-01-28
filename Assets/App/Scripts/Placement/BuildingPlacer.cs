using App.Scripts.Buildings;
using App.Scripts.Grid;
using UnityEngine;
using Zenject;

namespace App.Scripts.Placement
{
    public class BuildingPlacer
    {
        private readonly GridData _gridData;
        private readonly IBuildingFactory _buildingFactory; // Добавляем зависимость

        public BuildingPlacer(GridData gridData, IBuildingFactory buildingFactory)
        {
            _gridData = gridData;
            _buildingFactory = buildingFactory; // Инжектим фабрику
        }

        public void InstantiateAndPlaceBuilding(
            Building buildingPrefab, 
            GridManager gridManager, 
            Vector3Int gridPosition,
            Transform parentTransform)
        {
            var gridSize = gridManager.GridData.GridSize;
            var gridOffset = new Vector3((float)gridSize.x / 2, 0, (float)gridSize.y / 2);

            var buildingPosition = new Vector3(
                gridPosition.x - gridOffset.x, 
                0, 
                gridPosition.z - gridOffset.z
            );
            
            // Заменяем Object.Instantiate на фабрику
            Building jsonBuilding = _buildingFactory.Create(buildingPrefab, parentTransform);
            jsonBuilding.transform.position = buildingPosition;
            
            _gridData.AddObjectAt(jsonBuilding, gridPosition);
        }
    }
}