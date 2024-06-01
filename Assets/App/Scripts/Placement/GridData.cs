using System;
using System.Collections.Generic;
using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class GridData
    {
        private Dictionary<Vector3Int, PlacementData> _placedObjects = new();
        private Vector2Int _gridSize;
        private Vector2Int _gridOffset;

        public GridData(Vector2Int gridSize)
        {
            _gridSize = gridSize;
            _gridOffset = -(_gridSize / 2);
        }

        public void AddObjectAt(Vector3Int gridPosition, Vector2 objectSize, int id, Building placedObject)
        {
            List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
            PlacementData data = new PlacementData(positionsToOccupy, id, placedObject);
            foreach (var position in positionsToOccupy)
            {
                if (_placedObjects.ContainsKey(position))
                {
                    throw new Exception($"Dictionary already contains this cell position {position}");
                }

                _placedObjects[position] = data;
            }
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

        public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2 objectSize)
        {
            List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
            foreach (var position in positionsToOccupy)
            {
                if (!IsWithinBounds(position) || _placedObjects.ContainsKey(position))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsWithinBounds(Vector3Int position)
        {
            Vector3Int offsetPosition = position - new Vector3Int(_gridOffset.x, 0, _gridOffset.y);
            return offsetPosition.x >= 0 && offsetPosition.x < _gridSize.x && offsetPosition.z >= 0 && offsetPosition.z < _gridSize.y;
        }

        public Building GetPlacedObject(Vector3Int gridPosition)
        {
            if (!_placedObjects.ContainsKey(gridPosition))
            {
                return null;
            }

            return _placedObjects[gridPosition].PlacedObject;
        }

        public void RemoveObjectAt(Vector3Int gridPosition)
        {
            foreach (var position in _placedObjects[gridPosition].OccupiesPositions)
            {
                _placedObjects.Remove(position);
            }
        }
    }
}





