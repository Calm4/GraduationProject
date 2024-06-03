using App.Scripts.Buildings;
using App.Scripts.Resources;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class PlacementState : IBuildingState
    {
        private ResourcesManager _resourcesManager;
        private BuildSystem _buildSystem;
        private GridLayout _grid;
        private PreviewSystem _previewSystem;
        private GridData _floorData;
        private GridData _furnitureData;
        private SoundFeedback _soundFeedback;
        
        private BuildingManager _buildingManager;

        private Building _buildingPrefab;

        public PlacementState(ResourcesManager resourcesManager, BuildSystem buildSystem,Building buildingPrefab, GridLayout grid, PreviewSystem previewSystem,
            GridData floorData, GridData furnitureData, BuildingManager buildingManager, SoundFeedback soundFeedback)
        {
            _resourcesManager = resourcesManager;
            _buildingManager = buildingManager;
            
            _buildSystem = buildSystem;
            
            _buildingPrefab = buildingPrefab;
            _buildingPrefab.Initialize(buildingPrefab.BuildingConfig);
            
            _grid = grid;
            _previewSystem = previewSystem;
            _floorData = floorData;
            _furnitureData = furnitureData;
            _soundFeedback = soundFeedback;

            
            _previewSystem.StartShowingPlacementPreview(_buildingPrefab, _buildingPrefab.BuildingConfig.size);
        }
        
        public void EndState()
        {
            _previewSystem.StopShowingPreview();
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
            if (!_buildSystem.CanAccommodateBuilding(_buildingPrefab.BuildingConfig))
            {
                PlaySound(SoundType.WrongPlacement);
                return;
            }

            PlaySound(SoundType.Place);
            _resourcesManager.TakeAwayResourcesForConstruction(_buildingPrefab.BuildingConfig);

            Building creatableBuilding = _buildingManager.PlaceBuilding(_buildingPrefab,_grid.CellToWorld(gridPosition));

            GridData selectedData = GetSelectedGridData();
            selectedData.AddObjectAt(gridPosition, _buildingPrefab.BuildingConfig.size, _buildingPrefab.BuildingConfig.ID, creatableBuilding);

            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
        }
        private GridData GetSelectedGridData()
        {
            return _buildingPrefab.BuildingConfig.buildingType == BuildingType.Neutral ? _floorData : _furnitureData;
        }
        
        private bool CheckPlacementValidity(Vector3Int gridPosition)
        {
            GridData selectedData = _buildingPrefab.BuildingConfig.buildingType == BuildingType.Neutral ? _floorData : _furnitureData;
            return selectedData.CanPlaceObjectAt(gridPosition, _buildingPrefab.BuildingConfig.size);
        }

        private bool IsPlacementValid(Vector3Int gridPosition)
        {
            GridData selectedData = GetSelectedGridData();
            return selectedData.CanPlaceObjectAt(gridPosition, _buildingPrefab.BuildingConfig.size);
        }
        public void UpdateState(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition);
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
        }
        private void PlaySound(SoundType soundType)
        {
            _soundFeedback.PlaySound(soundType);
        }
    }
}
