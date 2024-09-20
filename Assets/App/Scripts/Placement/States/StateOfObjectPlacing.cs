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
        private readonly GridLayout _gridLayout;
        private readonly BuildingPreview _buildingPreview;
         private readonly GridData _gridData;
        private readonly SoundFeedback _soundFeedback;
        
        private readonly BuildingManager _buildingManager;

        private readonly Building _buildingPrefab;

        

        public StateOfObjectPlacing(ResourcesManager resourcesManager,Building buildingPrefab, GridLayout gridLayout, BuildingPreview buildingPreview,
             GridData gridData, BuildingManager buildingManager, SoundFeedback soundFeedback)
        {
            _resourcesManager = resourcesManager;
            _buildingManager = buildingManager;
            
            _buildingPrefab = buildingPrefab;
            _buildingPrefab.Initialize(buildingPrefab.BuildingConfig);
            
            _gridLayout = gridLayout;
            _buildingPreview = buildingPreview;
            
            _gridData = gridData;
            _soundFeedback = soundFeedback;

            
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

            Building creatableBuilding = _buildingManager.PlaceBuilding(_buildingPrefab,_gridLayout.CellToWorld(gridPosition));

            GridData selectedData = GetSelectedGridData();
            selectedData.AddObjectAt(gridPosition, creatableBuilding);

            _buildingPreview.UpdatePosition(_gridLayout.CellToWorld(gridPosition), false);
        }
        private GridData GetSelectedGridData()
        {
            return _gridData;
        }
        
        private bool CheckPlacementValidity(Vector3Int gridPosition)
        {
            return _gridData.CanPlaceObjectAt(gridPosition, _buildingPrefab.BuildingConfig.size);
        }

        private bool IsPlacementValid(Vector3Int gridPosition)
        {
            GridData selectedData = GetSelectedGridData();
            return selectedData.CanPlaceObjectAt(gridPosition, _buildingPrefab.BuildingConfig.size);
        }
        public void UpdateState(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition);
            _buildingPreview.UpdatePosition(_gridLayout.CellToWorld(gridPosition), placementValidity);
        }
        private void PlaySound(SoundType soundType)
        {
            _soundFeedback.PlaySound(soundType);
        }
    }
}
