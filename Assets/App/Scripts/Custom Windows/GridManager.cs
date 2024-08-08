using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using UnityEngine;

namespace App.Scripts.Grid
{
    public class GridManager
    {
        private bool[,] _grid;

        public GridManager(Vector2Int gridSize)
        {
            _grid = new bool[gridSize.x, gridSize.y];
        }

        public bool IsWithinGrid(Vector2Int position, Vector2Int gridSize)
            => position.x < gridSize.x && position.y < gridSize.y;

        public bool IsGridSizeValid(Vector2Int newSize)
            => newSize.x != _grid.GetLength(0) || newSize.y != _grid.GetLength(1);

        public bool CanPlaceObject(BasicBuildingConfig buildingConfig, Vector2Int position, Vector2Int gridSize)
        {
            for (int i = 0; i < buildingConfig.size.x; i++)
            {
                for (int j = 0; j < buildingConfig.size.y; j++)
                {
                    int posX = position.x + i;
                    int posY = position.y + j;

                    if (posX >= gridSize.x || posY >= gridSize.y || _grid[posX, posY])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void MarkOccupiedCells(BasicBuildingConfig buildingConfig, Vector3Int position, bool isOccupied)
        {
            for (int x = 0; x < buildingConfig.size.x; x++)
            {
                for (int y = 0; y < buildingConfig.size.y; y++)
                {
                    int gridX = position.x + x;
                    int gridY = position.y + y;

                    if (gridX >= 0 && gridX < _grid.GetLength(0) && gridY >= 0 && gridY < _grid.GetLength(1))
                    {
                        _grid[gridX, gridY] = isOccupied;
                    }
                }
            }
        }

        public void InitializeGrid(Vector2Int gridSize, IEnumerable<GridObjectData> gridObjects)
        {
            _grid = new bool[gridSize.x, gridSize.y];
            foreach (var obj in gridObjects)
            {
                if (IsWithinGrid((Vector2Int)obj.position, gridSize))
                {
                    MarkOccupiedCells(obj.buildingConfig, obj.position, true);
                }
            }
        }
    }
}
