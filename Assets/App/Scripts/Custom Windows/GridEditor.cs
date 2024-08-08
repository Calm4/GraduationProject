using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using UnityEngine;

public class GridEditor
{
    public int MinGridSize { get; private set; } = 1;
    public int MaxGridSize { get; private set; } = 50;
    
    private GridDataSO _gridData;
    private bool[,] _grid;

    public GridEditor(GridDataSO gridData)
    {
        _gridData = gridData;
    }

    public void InitializeGrid()
    {
        _grid = new bool[_gridData.gridSize.x, _gridData.gridSize.y];
        UpdateOccupiedCells();
    }

    public void UpdateOccupiedCells()
    {
        foreach (var obj in _gridData.gridObjects)
        {
            if (obj.position.x < _gridData.gridSize.x && obj.position.y < _gridData.gridSize.y)
            {
                MarkCellsOccupied(obj.buildingConfig, obj.position, true);
            }
        }
    }

    public bool CanPlaceBuilding(BasicBuildingConfig buildingConfig, Vector2Int position)
    {
        for (int i = 0; i < buildingConfig.size.x; i++)
        {
            for (int j = 0; j < buildingConfig.size.y; j++)
            {
                int posX = position.x + i;
                int posY = position.y + j;

                if (posX >= _gridData.gridSize.x || posY >= _gridData.gridSize.y || _grid[posX, posY])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void PlaceBuilding(BasicBuildingConfig buildingConfig, Vector3Int position)
    {
        if (CanPlaceBuilding(buildingConfig, new Vector2Int(position.x, position.y)))
        {
            RemoveBuildingAtPosition(new Vector2Int(position.x, position.y));

            var newObject = new GridObjectData(buildingConfig, position);
            _gridData.gridObjects.Add(newObject);

            MarkCellsOccupied(buildingConfig, position, true);
        }
    }

    public void RemoveBuildingAtPosition(Vector2Int position)
    {
        var objectToRemove = _gridData.gridObjects.Find(obj =>
            obj.position.x <= position.x && position.x < obj.position.x + obj.buildingConfig.size.x &&
            obj.position.y <= position.y && position.y < obj.position.y + obj.buildingConfig.size.y);

        if (objectToRemove != null)
        {
            MarkCellsOccupied(objectToRemove.buildingConfig, objectToRemove.position, false);
            _gridData.gridObjects.Remove(objectToRemove);
        }
    }

    private void MarkCellsOccupied(BasicBuildingConfig buildingConfig, Vector3Int position, bool isOccupied)
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

    public void ClearGrid()
    {
        _gridData.ClearGrid();
        InitializeGrid();
    }
}
