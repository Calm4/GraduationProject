using App.Scripts.Buildings;
using App.Scripts.Placement.Grid;
using App.Scripts.Resources;
using App.Scripts.Sound;
using UnityEngine;

namespace App.Scripts.Placement.Placement.States
{
    public class StateOfObjectRemoving : IBuildingState
    {
        private readonly ResourcesManager _resourcesManager;
        private readonly GridLayout _grid;
        private readonly BuildingPreview _buildingPreview;
        private readonly GridData _floorData;
        private readonly GridData _furnitureData;
        private readonly BuildingManager _buildingManager;
        private readonly SoundFeedback _soundFeedback;

        public StateOfObjectRemoving(ResourcesManager resourcesManager,BuildingManager buildingManager, GridLayout grid, BuildingPreview buildingPreview, GridData floorData, GridData furnitureData, SoundFeedback soundFeedback)
        {
            _resourcesManager = resourcesManager;
            _buildingManager = buildingManager;
            
            _grid = grid;
            _buildingPreview = buildingPreview;
            
            _floorData = floorData;
            _furnitureData = furnitureData;
            
            _soundFeedback = soundFeedback;

            buildingPreview.StartShowingRemovePreview();
        }

        public void EndState()
        {
            _buildingPreview.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            GridData selectedData = null;
            if (!_furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            {
                selectedData = _furnitureData;
            }
            else if (!_floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            {
                selectedData = _floorData;
            }

            if (selectedData == null)
            {
                return;
            }

            Building placedObject = selectedData.GetPlacedObject(gridPosition);
            if (placedObject == null)
            {
                return;
            }

            _resourcesManager.ReturnHalfOfResourcesForDestructionBuilding(placedObject.BuildingConfig);

            selectedData.RemoveObjectAt(gridPosition);

            _buildingManager.RemoveBuilding(placedObject);

            Vector3 cellPosition = _grid.CellToWorld(gridPosition);
            _buildingPreview.UpdatePosition(cellPosition, CheckIsSelectionIsValid(gridPosition));
        }


        private bool CheckIsSelectionIsValid(Vector3Int gridPosition)
        {
            return !(_furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) &&
                     _floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            bool validity = CheckIsSelectionIsValid(gridPosition);
            _buildingPreview.UpdatePosition(_grid.CellToWorld(gridPosition),validity);
        }
        
        
    }
}