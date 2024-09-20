using System.Collections.Generic;
using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using UnityEngine;

namespace App.Scripts.Custom_Windows
{
    public class GridMapWindow
    {
        private bool[,] _grid;

        public GridMapWindow(Vector2Int gridSize)
        {
            _grid = new bool[gridSize.x, gridSize.y];
        }

        private bool IsWithinGrid(Vector2Int position, Vector2Int gridSize) => position.x < gridSize.x && position.y < gridSize.y;
        public bool IsGridSizeValid(Vector2Int newSize) => newSize.x != _grid.GetLength(0) || newSize.y != _grid.GetLength(1);

        public bool CanPlaceObject(Building building, Vector2Int position, Vector2Int gridSize)
        {
            for (int i = 0; i < building.BuildingConfig.size.x; i++)
            {
                for (int j = 0; j < building.BuildingConfig.size.y; j++)
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

        public void MarkOccupiedCells(Building building, Vector3Int position, bool isOccupied)
        {
            for (int x = 0; x < building.BuildingConfig.size.x; x++)
            {
                for (int y = 0; y < building.BuildingConfig.size.y; y++)
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
                if (IsWithinGrid((Vector2Int)obj.Position, gridSize))
                {
                    MarkOccupiedCells(obj.Building, obj.Position, true);
                }
            }
        }
    }
}
