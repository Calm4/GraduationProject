using System.Collections.Generic;
using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.GameInput;
using App.Scripts.Grid;
using App.Scripts.Placement.States;
using App.Scripts.Resources;
using App.Scripts.Sound;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class PlacementManager : MonoBehaviour
    {
        [Title("Grid"), Space] 
        [SerializeField][VerticalGroup("Grid Settings")] private GridLayout grid;
        [SerializeField][VerticalGroup("Grid Settings")] private Vector2Int gridSize;
        [SerializeField][VerticalGroup("Grid Settings")] private GameObject gridVisualization;
        [SerializeField][VerticalGroup("Grid Settings")] private GameObject floor;
        
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

        [SerializeField] private TextAsset jsonFile;

        private Vector3Int _lastDetectedPosition = Vector3Int.zero;
        
        [Button]
        public void GetGridData()
        {
            _furnitureData.PrintGridState();
        }

        private void Start()
        {
            StopPlacement();

            if (jsonFile != null)
            {
                LoadGridSizeFromJson(jsonFile.text);
            }

            _floorData = new GridData(gridSize);
            _furnitureData = new GridData(gridSize);
            GridSetup();

            if (jsonFile != null)
            {
                PlaceObjectsFromJson(jsonFile.text); 
            }

            inputManager.OnExit += StopPlacement;
        }

        private void LoadGridSizeFromJson(string jsonString)
        {
            GridDataJson gridDataJson = JsonConvert.DeserializeObject<GridDataJson>(jsonString);

            if (gridDataJson != null && gridDataJson.gridSize != null)
            {
                gridSize = new Vector2Int(gridDataJson.gridSize.x, gridDataJson.gridSize.y);
            }
            else
            {
                Debug.LogWarning("No grid size found in JSON, using default size.");
            }
        }

        private void PlaceObjectsFromJson(string jsonString)
        {
            GridDataJson gridDataJson = JsonConvert.DeserializeObject<GridDataJson>(jsonString);

            foreach (var gridObject in gridDataJson.gridObjects)
            {
                var buildingConfig = buildingManager.GetBuildingConfigById(gridObject.buildingConfig.instanceID);
                if (buildingConfig != null)
                {
                    Vector3Int gridPosition = new Vector3Int(gridObject.position.x, gridObject.position.y, gridObject.position.z);
                    if (_furnitureData.CanPlaceObjectAt(gridPosition, buildingConfig.size))
                    {
                        Building building = buildingManager.PlaceBuilding(buildingPrefab, grid.CellToWorld(gridPosition));
                        _furnitureData.AddObjectAt(gridPosition, buildingConfig.size, building);
                    }
                    else
                    {
                        Debug.LogWarning($"Cannot place building at {gridPosition}, position is occupied or out of bounds.");
                    }
                }
                else
                {
                    Debug.LogWarning($"No building config found for ID {gridObject.buildingConfig.instanceID}");
                }
            }
        }

        
        
        public void StartPlacement(BasicBuildingConfig buildingConfig)
        {
            StopPlacement();
            gridVisualization.SetActive(true);

            buildingPrefab.Initialize(buildingConfig);

            _buildingState = new StateOfObjectPlacing(resourcesManager, buildingPrefab, grid, buildingPreview,
                _floorData, _furnitureData, buildingManager, soundFeedback);

            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }

        public void StartRemoving()
        {
            StopPlacement();
            gridVisualization.SetActive(true);

            _buildingState = new StateOfObjectRemoving(resourcesManager, buildingManager, grid, buildingPreview, _floorData,
                _furnitureData, soundFeedback);

            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }

        [Button, VerticalGroup("Grid Settings")]
        private void GridSetup()
        {
            var gridOffset = Vector3.zero - new Vector3((float)gridSize.x/2, 0, (float)gridSize.y/2);
            grid.transform.position = gridOffset;

            var gridVisualizationSize = new Vector3((float)gridSize.x / 10, 1, (float)gridSize.y / 10);
            gridVisualization.transform.localScale = gridVisualizationSize;
            floor.transform.localScale = gridVisualizationSize;
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
    [System.Serializable]
    public class GridDataJson
    {
        public Vector2IntJson gridSize;
        public List<GridObjectJson> gridObjects;
    }

    [System.Serializable]
    public class Vector2IntJson
    {
        public int x;
        public int y;
    }

    [System.Serializable]
    public class GridObjectJson
    {
        public BuildingConfigJson buildingConfig;
        public Vector3IntJson position;
    }

    [System.Serializable]
    public class BuildingConfigJson
    {
        public int instanceID;
    }

    [System.Serializable]
    public class Vector3IntJson
    {
        public int x;
        public int y;
        public int z;
    }
}