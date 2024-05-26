using App.Scripts;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GridLayout grid;
    [SerializeField] private Vector2Int gridSize; // Добавьте это поле для хранения размеров сетки
    private Vector2Int _gridOffset; // Добавьте это поле для хранения смещения сетки

    [SerializeField] private ObjectsDatabaseSO databaseSo;
    [SerializeField] private GameObject gridVisualization;

    private GridData _floorData;
    private GridData _furnitureData;

    [SerializeField] private PreviewSystem previewSystem;
    [SerializeField] private ObjectPlacer objectPlacer;

    private Vector3Int _lastDetectedPosition = Vector3Int.zero;

    private IBuildingState _buildingState;

    [SerializeField] private SoundFeedback soundFeedback;

    private void Start()
    {
        StopPlacement();
        _floorData = new GridData(gridSize); // Передайте размеры и смещение сетки в конструктор GridData
        _furnitureData = new GridData(gridSize); // Передайте размеры и смещение сетки в конструктор GridData
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        _buildingState = new PlacementState(id, grid, previewSystem, databaseSo, _floorData, _furnitureData, objectPlacer, soundFeedback);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        _buildingState = new RemovingState(grid, previewSystem, _floorData, _furnitureData, objectPlacer, soundFeedback);

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

        _buildingState.OnAction(gridPosition);
    }

    private void StopPlacement()
    {
        if (_buildingState == null)
        {
            return;
        }
        gridVisualization.SetActive(false);
        _buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        _lastDetectedPosition = Vector3Int.zero;
        _buildingState = null;
    }

    private void Update()
    {
        if (_buildingState == null)
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if (_lastDetectedPosition != gridPosition)
        {
            _buildingState.UpdateState(gridPosition);
            _lastDetectedPosition = gridPosition;
        }
    }
}


