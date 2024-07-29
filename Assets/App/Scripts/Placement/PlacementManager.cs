using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.GameInput;
using App.Scripts.Resources;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class PlacementManager : MonoBehaviour
    {
        [Title("Grid"), Space] 
        [SerializeField] private GridLayout grid;

        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private GameObject gridVisualization;

        private Vector2Int _gridOffset;
        private GridData _floorData;
        private GridData _furnitureData;

        [Title("Managers"), Space] 
        [SerializeField] private InputManager inputManager;
        [SerializeField] private BuildingManager buildingManager;
        [SerializeField] private ResourcesManager resourcesManager;

        [Title("Buildings"), Space] [SerializeField]
        private BuildingPreview buildingPreview;

        [SerializeField] private Building buildingPrefab;

        private IBuildingState _buildingState;

        [Title("Sound"), Space] 
        [SerializeField] private SoundFeedback soundFeedback;


        private Vector3Int _lastDetectedPosition = Vector3Int.zero;

        [Button]
        public void GetGridData()
        {
            _furnitureData.PrintGridState();
        }

        private void Start()
        {
            StopPlacement();

            _floorData = new GridData(gridSize);
            _furnitureData = new GridData(gridSize);

            inputManager.OnExit += StopPlacement;
        }

        public void StartPlacement(BasicBuildingConfig buildingConfig)
        {
            StopPlacement();
            gridVisualization.SetActive(true);

            buildingPrefab.Initialize(buildingConfig);

            _buildingState = new PlacementState(resourcesManager, buildingPrefab, grid, buildingPreview,
                _floorData, _furnitureData, buildingManager, soundFeedback);

            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }

        public void StartRemoving()
        {
            StopPlacement();
            gridVisualization.SetActive(true);

            _buildingState = new RemovingState(resourcesManager, buildingManager, grid, buildingPreview, _floorData,
                _furnitureData, soundFeedback);

            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }

        [Button]
        private void GridInfo()
        {
            Debug.Log(grid);
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
}