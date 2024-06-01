using System.Collections.Generic;
using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class PlacementData
    {
        public List<Vector3Int> OccupiesPositions { get; }
        public int ID { get; }
        public Building PlacedObject { get; }

        public PlacementData(List<Vector3Int> occupiesPositions, int id, Building placedObject)
        {
            OccupiesPositions = occupiesPositions;
            ID = id;
            PlacedObject = placedObject;
        }
    }
}