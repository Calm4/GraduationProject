using App.Scripts.Buildings;

namespace App.Scripts.Placement
{
    public class GridCell
    {
        public bool IsOccupied { get; private set; }
        public Building OccupyingBuilding { get; private set; }
        public GridCell()
        {
            IsOccupied = false;
            OccupyingBuilding = null;
        }
        public void Occupy(Building building)
        {
            IsOccupied = true;
            OccupyingBuilding = building;
        }

        public void Vacate()
        {
            IsOccupied = false;
            OccupyingBuilding = null;
        }
    }
}