using System.Collections.Generic;
using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class GridData
    {
        private readonly GridCell[,] gridCells;
        //private readonly Dictionary<Vector3Int, PlacementData> _placedObjects = new();
        private Vector2Int _gridSize;
        private Vector2Int _gridOffset;
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
            List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
            foreach (var position in positionsToOccupy)
            {
                gridCells[position.x, position.z].Occupy(building);
            }
        }
        public void RemoveObjectAt(Vector3Int gridPosition, Vector2 objectSize)
        {
            List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
            foreach (var position in positionsToOccupy)
            {
                gridCells[position.x, position.z].Vacate();
            }
        }
        public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2 objectSize)
        {
            List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
            foreach (var position in positionsToOccupy)
            {
                if (!IsWithinBounds(position))
                {
                    Debug.LogError($"Position {position} is out of bounds.");
                    return false;
                }

                if (gridCells[position.x, position.z].IsOccupied)
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
                for (int y = 0; y < objectSize.y; y++)
                {
                    positions.Add(gridPosition + new Vector3Int(x, 0, y));
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
        
    }
}





