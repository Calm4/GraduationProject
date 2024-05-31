using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class PlacementState : IBuildingState
    {
        private BuildSystem _buildSystem;
        private GridLayout _grid;
        private PreviewSystem _previewSystem;
        private GridData _floorData;
        private GridData _furnitureData;
        private ObjectPlacer _objectPlacer;
        private SoundFeedback _soundFeedback;
        private BasicBuildingConfig _basicBuildingConfig;

        private GameObject _buildingPrefab;

        public PlacementState(BuildSystem buildSystem,GameObject buildingPrefab, BasicBuildingConfig basicBuildingConfig, GridLayout grid, PreviewSystem previewSystem,
            GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer, SoundFeedback soundFeedback)
        {
            _buildSystem = buildSystem;
            _buildingPrefab = buildingPrefab;
            _basicBuildingConfig = basicBuildingConfig;
            _grid = grid;
            _previewSystem = previewSystem;
            _floorData = floorData;
            _furnitureData = furnitureData;
            _objectPlacer = objectPlacer;
            _soundFeedback = soundFeedback;

            /*if (_building == null || _building.ID != _buildingConfig.ID)
        {
            throw new System.Exception($"No building with ID {_buildingConfig.ID}");
        }*/
        
            _previewSystem.StartShowingPlacementPreview(_buildingPrefab, basicBuildingConfig.size);
        }

        public void EndState()
        {
            _previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition);
            if (!placementValidity)
            {
                _soundFeedback.PlaySound(SoundType.wrongPlacement);
                return;
            }

            _soundFeedback.PlaySound(SoundType.Place);
        
            var (creatableBuilding, index) = _objectPlacer.PlaceObject(_buildingPrefab, _grid.CellToWorld(gridPosition));

            MeshFilter meshFilter = creatableBuilding.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                meshFilter.mesh = _basicBuildingConfig.mesh;
            }

            MeshRenderer meshRenderer = creatableBuilding.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = _basicBuildingConfig.material;
            }
        
            GridData selectedData = _basicBuildingConfig.buildingType == BuildingType.Neutral ? _floorData : _furnitureData;
            selectedData.AddObjectAt(gridPosition, _basicBuildingConfig.size, _basicBuildingConfig.ID, index);

            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
        }

        private bool CheckPlacementValidity(Vector3Int gridPosition)
        {
            GridData selectedData = _basicBuildingConfig.buildingType == BuildingType.Neutral ? _floorData : _furnitureData;
            return selectedData.CanPlaceObjectAt(gridPosition, _basicBuildingConfig.size);
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition);
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
        }
    }
}
