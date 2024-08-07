using System;
using App.Scripts.Buildings.BuildingsConfigs;
using UnityEngine;

namespace App.Scripts.Grid
{
    [Serializable]
    public class GridObjectData
    {
        public BasicBuildingConfig buildingConfig; 
        public Vector3Int position;

        public GridObjectData(BasicBuildingConfig buildingConfig, Vector3Int position)
        {
            this.buildingConfig = buildingConfig;
            this.position = position;
        }
    }
}