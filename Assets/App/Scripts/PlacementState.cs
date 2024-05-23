using System.Collections;
using System.Collections.Generic;
using App.Scripts;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int _selectedObjectIndex = -1;
    private int _id;
    private GridLayout _grid;
    private PreviewSystem _previewSystem;
    private ObjectsDatabaseSO _databaseSo;
    private GridData _floorData;
    private GridData _furnitureData;
    private ObjectPlacer _objectPlacer;
    private SoundFeedback _soundFeedback;
    /*SoundFeedback soundFeedback;*/


    public PlacementState(int id, GridLayout grid, PreviewSystem previewSystem, ObjectsDatabaseSO databaseSo,
        GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer,SoundFeedback soundFeedback)
    {
        _id = id;
        _grid = grid;
        _previewSystem = previewSystem;
        _databaseSo = databaseSo;
        _floorData = floorData;
        _furnitureData = furnitureData;
        _objectPlacer = objectPlacer;
        _soundFeedback = soundFeedback;

        _selectedObjectIndex = databaseSo.objectsData.FindIndex(data => data.ID == id);
        if (_selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                databaseSo.objectsData[_selectedObjectIndex].Prefab,
                databaseSo.objectsData[_selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No objec with ID {id}");
        }
    }

    public void EndState()
    {
        _previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
        if (placementValidity == false)
        {
            _soundFeedback.PlaySound(SoundType.wrongPlacement);
            return;
        }
        _soundFeedback.PlaySound(SoundType.Place);
        int index = _objectPlacer.PlaceObject(_databaseSo.objectsData[_selectedObjectIndex].Prefab,
            _grid.CellToWorld(gridPosition));

        GridData selectedData = _databaseSo.objectsData[_selectedObjectIndex].ID == 0 ? _floorData : _furnitureData;
        
        selectedData.AddObjectAt(gridPosition, 
            _databaseSo.objectsData[_selectedObjectIndex].Size, _databaseSo.objectsData[_selectedObjectIndex].ID, index);

        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
    }
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = _databaseSo.objectsData[selectedObjectIndex].ID == 0 ?
            _floorData :
            _furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, _databaseSo.objectsData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);

        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
    }
}