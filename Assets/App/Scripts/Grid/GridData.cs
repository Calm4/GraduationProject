using System.Collections.Generic;
using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Grid
{
    public class GridData
    {
        private readonly GridCell[,] gridCells;
        private Vector2Int _gridSize;
        private Vector2Int _gridOffset;

        public GridData(Vector2Int gridSize)
        {
            _gridSize = gridSize;
            _gridOffset = Vector2Int.zero;

            gridCells = new GridCell[gridSize.x, gridSize.y];
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    gridCells[x, y] = new GridCell();
                }
            }
        }

        public void AddObjectAt(Vector3Int gridPosition, Vector2 objectSize, Building building)
        {
            foreach (var position in CalculatePositions(gridPosition, objectSize))
            {
                gridCells[position.x, position.z].Occupy(building, gridPosition);
            }
        }

        public void RemoveObjectAt(Vector3Int gridPosition)
        {
            GridCell initialCell = gridCells[gridPosition.x, gridPosition.z];
            Building building = initialCell.OccupyingBuilding;
            if (building == null)
            {
                return;
            }

            Vector3Int buildingOrigin = initialCell.BuildingOrigin;
            Vector2Int buildingSize = building.BuildingConfig.size;

            foreach (var position in CalculatePositions(buildingOrigin, buildingSize))
            {
                gridCells[position.x, position.z].Vacate();
            }
        }

        public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2 objectSize)
        {
            foreach (var position in CalculatePositions(gridPosition, objectSize))
            {
                if (!IsWithinBounds(position) || gridCells[position.x, position.z].IsOccupied)
                {
                    return false;
                }
            }
            return true;
        }

        private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2 objectSize)
        {
            List<Vector3Int> positions = new();
            for (int x = 0; x < objectSize.x; x++)
            {
                for (int z = 0; z < objectSize.y; z++)
                {
                    positions.Add(gridPosition + new Vector3Int(x, 0, z));
                }
            }
            return positions;
        }

        private bool IsWithinBounds(Vector3Int position)
        {
            Vector3Int offsetPosition = position - new Vector3Int(_gridOffset.x, 0, _gridOffset.y);
            return offsetPosition.x >= 0 && offsetPosition.x < _gridSize.x && offsetPosition.z >= 0 && offsetPosition.z < _gridSize.y;
        }

        public Building GetPlacedObject(Vector3Int gridPosition)
        {
            return gridCells[gridPosition.x, gridPosition.z].OccupyingBuilding;
        }
        public void PrintGridState()
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                string row = "";
                for (int x = 0; x < _gridSize.x; x++)
                {
                    row += gridCells[x, y].IsOccupied ? "[X]" : "[ ]";
                }
                Debug.Log(row);
            }
        }
    }
}
