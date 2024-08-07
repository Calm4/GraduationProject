using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class GridCell
    {
        public Building OccupyingBuilding { get; private set; }
        public Vector3Int BuildingOrigin { get; private set; }
        public bool IsOccupied => OccupyingBuilding != null;

        public void Occupy(Building building, Vector3Int origin)
        {
            OccupyingBuilding = building;
            BuildingOrigin = origin;
        }

        public void Vacate()
        {
            OccupyingBuilding = null;
            BuildingOrigin = default;
        }
    }
}