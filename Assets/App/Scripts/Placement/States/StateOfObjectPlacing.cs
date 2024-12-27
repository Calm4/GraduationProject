using App.Scripts.Buildings;
using App.Scripts.GameResources;
using App.Scripts.Grid;
using App.Scripts.Sound;
using UnityEngine;

namespace App.Scripts.Placement.States
{
    public class StateOfObjectPlacing : IBuildingState
    {
        private readonly ResourcesManager _resourcesManager;
        private readonly BuildingManager _buildingManager;
        private readonly GridManager _gridManager;
        private readonly BuildingPreview _buildingPreview;
        private readonly SoundFeedbackManager _soundFeedbackManager;
        

        private readonly Building _buildingPrefab;

        

        public StateOfObjectPlacing(ResourcesManager resourcesManager,BuildingManager buildingManager,GridManager gridManager, Building buildingPrefab, BuildingPreview buildingPreview, SoundFeedbackManager soundFeedbackManager)
        {
            _resourcesManager = resourcesManager;
            _buildingManager = buildingManager;
            _gridManager = gridManager;
            
            _buildingPrefab = buildingPrefab;
            
            _buildingPreview = buildingPreview;
            
            _soundFeedbackManager = soundFeedbackManager;

            
            _buildingPreview.StartShowingPlacementPreview(_buildingPrefab, _buildingPrefab.BuildingConfig.size);
        }
        
        public void EndState()
        {
            _buildingPreview.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            if (!IsPlacementValid(gridPosition))
            {
                PlaySound(SoundType.WrongPlacement);
                return;
            }

            PlaceBuilding(gridPosition);
        }
        
        private void PlaceBuilding(Vector3Int gridPosition)
        {
            if (!_resourcesManager.CalculatePossibilityOfPlacingBuilding(_buildingPrefab.BuildingConfig))
            {
                PlaySound(SoundType.WrongPlacement);
                return;
            }

            PlaySound(SoundType.Place);
            _resourcesManager.TakeAwayResourcesForConstruction(_buildingPrefab.BuildingConfig);

            Building creatableBuilding = _buildingManager.PlaceBuilding(_buildingPrefab, _gridManager.GridLayout.CellToWorld(gridPosition));

            GridData selectedData = GetSelectedGridData();
            selectedData.AddObjectAt(creatableBuilding, gridPosition);

            _buildingPreview.UpdatePosition(_gridManager.GridLayout.CellToWorld(gridPosition), false);
        }
        private GridData GetSelectedGridData()
        {
            return _gridManager.GridData;
        }
        
        private bool CheckPlacementValidity(Vector3Int gridPosition)
        {
            return GetSelectedGridData().CanPlaceObjectAt(gridPosition, _buildingPrefab.BuildingConfig.size);
        }

        private bool IsPlacementValid(Vector3Int gridPosition)
        {
            GridData selectedData = GetSelectedGridData();
            return selectedData.CanPlaceObjectAt(gridPosition, _buildingPrefab.BuildingConfig.size);
        }
        public void UpdateState(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition);
            _buildingPreview.UpdatePosition(_gridManager.GridLayout.CellToWorld(gridPosition), placementValidity);
        }
        private void PlaySound(SoundType soundType)
        {
            _soundFeedbackManager.PlaySound(soundType);
        }
    }
}
