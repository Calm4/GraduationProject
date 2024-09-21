using System.Collections.Generic;
using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Grid
{
    public class GridData
    {
        private readonly GridCell[,] _gridCells;
        public Vector2Int GridSize { get; }

        public GridData(Vector2Int gridSize)
        {
            GridSize = gridSize;

            _gridCells = new GridCell[gridSize.x, gridSize.y];
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    _gridCells[x, y] = new GridCell();
                }
            }
        }

        public void AddObjectAt(Vector3Int gridPosition, Building building)
        {
            foreach (var position in CalculatePositions(gridPosition, building.BuildingConfig.size))
            {
                if (IsWithinBounds(position))
                {
                    _gridCells[position.x, position.z].Occupy(building, gridPosition);
                }
            }
        }
        public void RemoveObjectAt(Vector3Int gridPosition)
        {
            GridCell initialCell = _gridCells[gridPosition.x, gridPosition.z];
            Building building = initialCell.OccupyingBuilding;
            if (!building)
            {
                return;
            }

            Vector3Int buildingOrigin = initialCell.BuildingOrigin;
            Vector2Int buildingSize = building.BuildingConfig.size;

            foreach (var position in CalculatePositions(buildingOrigin, buildingSize))
            {
                _gridCells[position.x, position.z].Vacate();
            }

            Object.Destroy(building.gameObject);
        }

        public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2 objectSize)
        {
            foreach (var position in CalculatePositions(gridPosition, objectSize))
            {
                if (!IsWithinBounds(position) || _gridCells[position.x, position.z].IsOccupied)
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
            return position.x >= 0 && position.x < GridSize.x && position.z >= 0 &&
                   position.z < GridSize.y;
        }

        public Building GetPlacedObject(Vector3Int gridPosition)
        {
            return _gridCells[gridPosition.x, gridPosition.z].OccupyingBuilding;
        }

        public void PrintGridState()
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                string row = "";
                for (int x = 0; x < GridSize.x; x++)
                {
                    row += _gridCells[x, y].IsOccupied ? "[X]" : "[ ]";
                }

                Debug.Log(row);
            }
        }
    }
}