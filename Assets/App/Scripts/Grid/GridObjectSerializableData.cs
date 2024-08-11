using System;
using UnityEngine;

namespace App.Scripts.Grid
{
    [Serializable]
    public class GridObjectSerializableData
    {
        public int buildingConfigID;
        public Vector3Int position;

        public GridObjectSerializableData(int buildingConfigID, Vector3Int position)
        {
            this.buildingConfigID = buildingConfigID;
            this.position = position;
        }
    }
}