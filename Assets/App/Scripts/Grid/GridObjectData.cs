using System;
using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Grid
{
    [Serializable]
    public class GridObjectData
    {
        public Building Building; 
        public Vector3Int Position;

        public GridObjectData(Building buildingConfig, Vector3Int position)
        {
            Building = buildingConfig;
            Position = position;
        }
    }
}