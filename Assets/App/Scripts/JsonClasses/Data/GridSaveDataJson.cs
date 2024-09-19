using System;
using System.Collections.Generic;
using App.Scripts.Grid;
using UnityEngine;

namespace App.Scripts.JsonClasses.Data
{
    [Serializable]
    public class GridSaveDataJson
    {
        public Vector2Int gridSize;
        public List<GridObjectSerializableData> gridObjects;
    }
}