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
        private readonly GridLayout _gridLayout;
        private readonly BuildingPreview _buildingPreview;
        private readonly GridData _gridData;
        private readonly BuildingManager _buildingManager;
        private readonly SoundFeedback _soundFeedback;

        public StateOfObjectRemoving(ResourcesManager resourcesManager,BuildingManager buildingManager, GridLayout gridLayout, BuildingPreview buildingPreview, GridData gridData, SoundFeedback soundFeedback)
        {
            _resourcesManager = resourcesManager;
            _buildingManager = buildingManager;
            
            _gridLayout = gridLayout;
            _buildingPreview = buildingPreview;
            
            _gridData = gridData;
            
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
            if (!_gridData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            {
                selectedData = _gridData;
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

            Vector3 cellPosition = _gridLayout.CellToWorld(gridPosition);
            _buildingPreview.UpdatePosition(cellPosition, CheckIsSelectionIsValid(gridPosition));
        }


        private bool CheckIsSelectionIsValid(Vector3Int gridPosition)
        {
            return _gridData.CanPlaceObjectAt(gridPosition, Vector2Int.one);
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            bool validity = CheckIsSelectionIsValid(gridPosition);
            _buildingPreview.UpdatePosition(_gridLayout.CellToWorld(gridPosition),validity);
        }
        
        
    }
}