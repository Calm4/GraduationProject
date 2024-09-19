using System;
using System.Collections.Generic;
using App.Scripts.Grid;

namespace App.Scripts.JsonClasses.Data
{
    [Serializable]
    public class GridObjectContainerJson
    {
        public List<GridObjectSerializableData> gridObjects;
    }
}