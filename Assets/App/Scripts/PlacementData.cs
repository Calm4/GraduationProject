using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts
{
    public class PlacementData
    {
        public List<Vector3Int> occupiesPositions;
        public int ID { get; private set; }
        public int PlacedObjectIndex { get; private set; }

        public PlacementData(List<Vector3Int> occupiesPositions, int ID, int placedObjectIndex)
        {
            this.occupiesPositions = occupiesPositions;
            this.ID = ID;
            this.PlacedObjectIndex = placedObjectIndex;
        }
    }
}