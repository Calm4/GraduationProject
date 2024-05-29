using App.Scripts;
using App.Scripts.Resources;
using App.Scripts.Buildings;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private GridLayout _grid;
    private PreviewSystem _previewSystem;
    private GridData _floorData;
    private GridData _furnitureData;
    private ObjectPlacer _objectPlacer;
    private SoundFeedback _soundFeedback;
    private BuildingConfig _buildingConfig;

    private GameObject _buildingPrefab;

    public PlacementState(GameObject buildingPrefab, BuildingConfig buildingConfig, GridLayout grid, PreviewSystem previewSystem,
        GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer, SoundFeedback soundFeedback)
    {
        _buildingPrefab = buildingPrefab;
        _buildingConfig = buildingConfig;
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
        
        _previewSystem.StartShowingPlacementPreview(_buildingPrefab, buildingConfig.size);
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
            meshFilter.mesh = _buildingConfig.mesh;
        }

        MeshRenderer meshRenderer = creatableBuilding.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = _buildingConfig.material;
        }
        
        GridData selectedData = _buildingConfig.buildingType == BuildingType.Neutral ? _floorData : _furnitureData;
        selectedData.AddObjectAt(gridPosition, _buildingConfig.size, _buildingConfig.ID, index);

        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition)
    {
        GridData selectedData = _buildingConfig.buildingType == BuildingType.Neutral ? _floorData : _furnitureData;
        return selectedData.CanPlaceObjectAt(gridPosition, _buildingConfig.size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
    }
}
