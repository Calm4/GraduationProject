using App.Scripts.Buildings;
using App.Scripts.Resources;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class RemovingState : IBuildingState
    {
        private ResourcesManager _resourcesManager;
        private GridLayout _grid;
        private PreviewSystem _previewSystem;
        private GridData _floorData;
        private GridData _furnitureData;
        private BuildingManager _buildingManager;
        private SoundFeedback _soundFeedback;

        public RemovingState(ResourcesManager resourcesManager, GridLayout grid, PreviewSystem previewSystem, GridData floorData, GridData furnitureData, BuildingManager buildingManager, SoundFeedback soundFeedback)
        {
            _resourcesManager = resourcesManager;
            _grid = grid;
            _previewSystem = previewSystem;
            _floorData = floorData;
            _furnitureData = furnitureData;
            _buildingManager = buildingManager;
            _soundFeedback = soundFeedback;

            previewSystem.StartShowingRemovePreview();
        }

        public void EndState()
        {
            _previewSystem.StopShowingPreview();
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
            _previewSystem.UpdatePosition(cellPosition, CheckIsSelectionIsValid(gridPosition));
        }

        private bool CheckIsSelectionIsValid(Vector3Int gridPosition)
        {
            return !(_furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) &&
                     _floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            bool validity = CheckIsSelectionIsValid(gridPosition);
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition),validity);
        }
    }
}