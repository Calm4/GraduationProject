using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.GameResources;
using App.Scripts.Grid;
using App.Scripts.Sound;
using UnityEngine;

namespace App.Scripts.Placement.States
{
    public class StateOfObjectRemoving : IBuildingState
    {
        private readonly ResourcesManager _resourcesManager;
        private readonly BuildingManager _buildingManager;
        private readonly GridManager _gridManager;
        
        private readonly BuildingPreview _buildingPreview;
        private readonly SoundFeedback _soundFeedback;

        public StateOfObjectRemoving(ResourcesManager resourcesManager,BuildingManager buildingManager, GridManager gridManager, BuildingPreview buildingPreview, SoundFeedback soundFeedback)
        {
            _resourcesManager = resourcesManager;
            _buildingManager = buildingManager;
            _gridManager = gridManager;
            _buildingPreview = buildingPreview;
            
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
            if (!_gridManager.GridData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            {
                selectedData = _gridManager.GridData;
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

            if (placedObject.BuildingConfig.buildingType == BuildingType.NonInteractive)
            {
                return;
            }

            _resourcesManager.ReturnHalfOfResourcesForDestructionBuilding(placedObject.BuildingConfig);

            selectedData.RemoveObjectAt(gridPosition);

            _buildingManager.RemoveBuilding(placedObject);

            Vector3 cellPosition = _gridManager.GridLayout.CellToWorld(gridPosition);
            _buildingPreview.UpdatePosition(cellPosition, CheckIsSelectionIsValid(gridPosition));
        }


        private bool CheckIsSelectionIsValid(Vector3Int gridPosition)
        {
            return _gridManager.GridData.CanPlaceObjectAt(gridPosition, Vector2Int.one);
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            bool validity = CheckIsSelectionIsValid(gridPosition);
            _buildingPreview.UpdatePosition(_gridManager.GridLayout.CellToWorld(gridPosition),validity);
        }
        
        
    }
}