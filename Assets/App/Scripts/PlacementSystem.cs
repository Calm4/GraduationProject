using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GridLayout grid;

    [SerializeField] private ObjectsDatabaseSO databaseSo;
    private int _selectedObjectIndex = -1;

    [SerializeField] private GameObject gridVisualization;

    private GridData _floorData;
    private GridData _furnitureData;

    private Renderer _previewRenderer;
    private List<GameObject> _placedGameObject = new();

    private void Start()
    {
        StopPlacement();
        _floorData = new GridData();
        _furnitureData = new GridData();
        _previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        _selectedObjectIndex = databaseSo.objectsData.FindIndex(data => data.ID == id);
        if (_selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {id}");
            return;
        }

        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
        if (!placementValidity) return;

        GameObject newObject = Instantiate(databaseSo.objectsData[_selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        _placedGameObject.Add(newObject);
        GridData selectedData = databaseSo.objectsData[_selectedObjectIndex].ID == 0 ? _floorData : _furnitureData;
        selectedData.AddObjectAt(gridPosition, databaseSo.objectsData[_selectedObjectIndex].Size,
            databaseSo.objectsData[_selectedObjectIndex].ID, _placedGameObject.Count - 1);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int i)
    {
        GridData selectedData = databaseSo.objectsData[_selectedObjectIndex].ID == 0 ? _floorData : _furnitureData;
        return selectedData.CanPlaceObjectAt(gridPosition, databaseSo.objectsData[_selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        _selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (_selectedObjectIndex < 0)
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
        _previewRenderer.material.color = placementValidity ? Color.white : Color.red;
        Debug.Log(_previewRenderer.material.color);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}