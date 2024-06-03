using System.Collections.Generic;
using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class PlacementData
    {
        public List<Vector3Int> OccupiesPositions { get; }
        public int ID { get; }
        public Building PlacedBuilding { get; }

        public PlacementData(List<Vector3Int> occupiesPositions, int id, Building placedBuilding)
        {
            OccupiesPositions = occupiesPositions;
            ID = id;
            PlacedBuilding = placedBuilding;
        }
    }
}