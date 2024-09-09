using System;
using System.Collections.Generic;
using App.Scripts.Grid;

namespace App.Scripts.Placement.JsonClasses
{
    [Serializable]
    public class GridObjectContainer
    {
        public List<GridObjectSerializableData> gridObjects;
    }
}