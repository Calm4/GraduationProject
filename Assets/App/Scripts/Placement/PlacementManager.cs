using System.Collections.Generic;
using System.Linq;
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
using UnityEngine.Serialization;

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

        [Title("Buildings"), Space] 
        [SerializeField] private BuildingPreview buildingPreview;
        [SerializeField] private Building buildingPrefab;
        [FormerlySerializedAs("buildingConfigsData")] [SerializeField] private BuildingsDataBase buildingsDataBase;
        private IBuildingState _buildingState;

        [Title("JSON FILE"), Space]
        [SerializeField] private TextAsset jsonFile;

        [Title("Sound"), Space] 
        [SerializeField] private SoundFeedback soundFeedback;

        private Vector3Int _lastDetectedPosition = Vector3Int.zero;
        private BuildingPlacer _buildingPlacer;
        
        
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
            _buildingPlacer = new BuildingPlacer(_furnitureData, gridSize); // Instantiate BuildingPlacer

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

        private void PlaceObjectsFromJson(string json)
        {
            var gridObjectsContainer = JsonUtility.FromJson<GridObjectContainer>(json);
            
            var gridObjects = new List<GridObjectData>();

            foreach (var gridObjectSerializable in gridObjectsContainer.gridObjects)
            {
                var config = buildingsDataBase.buildingConfigs.FirstOrDefault(b => b.ID == gridObjectSerializable.buildingConfigID);
                if (config != null)
                {
                    var gridObjectData = new GridObjectData(config, gridObjectSerializable.position);
                    gridObjects.Add(gridObjectData);
                }
                else
                {
                    Debug.LogWarning($"Building config with ID {gridObjectSerializable.buildingConfigID} not found.");
                }
            }

            // Передаем преобразованный список в InitializeGrid
            _floorData.InitializeGrid(gridObjects);

            // Теперь можно разместить объекты
            foreach (var gridObject in gridObjects)
            {
                _buildingPlacer.PlaceBuilding(gridObject.buildingConfig, gridObject.position);
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
            var gridOffset = new Vector3(-(float)gridSize.x / 2, 0, -(float)gridSize.y / 2);
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
    }

    [System.Serializable]
    public class Vector2IntJson
    {
        public int x;
        public int y;
    }
    
    [System.Serializable]
    public class GridObjectContainer
    {
        public List<GridObjectSerializableData> gridObjects;
    }


}