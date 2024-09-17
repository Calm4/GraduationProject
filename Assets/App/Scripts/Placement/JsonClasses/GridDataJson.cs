using System;
using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using App.Scripts.Placement.JsonClasses;
using UnityEngine;

[Serializable]
public class GridDataJson
{
    public Vector2IntJson gridSize;
    public List<GridObjectSerializableData> gridObjects; 

    public Vector3 spawnerPosition;
    public Vector3 castlePosition;

    // Method to add path to the grid
    public void AddPath(List<Vector3> pathPositions, BasicBuildingConfig pathBuildingConfig)
    {
        foreach (var pos in pathPositions)
        {
            gridObjects.Add(new GridObjectSerializableData(pathBuildingConfig.ID, Vector3Int.RoundToInt(pos)));
        }
    }
}