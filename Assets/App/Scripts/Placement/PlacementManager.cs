using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using App.Scripts.Input;
using App.Scripts.Placement.States;
using App.Scripts.Resources;
using App.Scripts.Sound;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class PlacementManager : MonoBehaviour
    {
        [SerializeField, LabelText(""), Space,Title("Current Placement Mode")] private PlacementMode currentPlacementMode = PlacementMode.None;

        [Title("Managers"), Space] [SerializeField]
        private InputManager inputManager;

        [SerializeField] private GridManager gridManager;
        [SerializeField] private BuildingManager buildingManager;
        [SerializeField] private ResourcesManager resourcesManager;

        [Title("Buildings"), Space] [SerializeField]
        private BuildingPreview buildingPreview;

        [SerializeField] private Building buildingPrefab;
        [SerializeField] private BuildingsDataBase buildingsDataBase;
        [SerializeField] private Transform buildingsFromJsonContainer;
        private IBuildingState _buildingState;

        [Title("JSON FILE"), Space] [SerializeField]
        private TextAsset jsonFile;

        [Title("Sound"), Space] [SerializeField]
        private SoundFeedback soundFeedback;

        private Vector3Int _lastDetectedPosition = Vector3Int.zero;
        private BuildingPlacer _buildingPlacer;
        
        private Vector2Int _gridOffset;
        private GridData _gridData;

        public event Action<bool> OnChangeGridVisualizationVisibility;

        private enum PlacementMode
        {
            None,
            Building,
            Removing
        }
        
        [Button]
        public void GetGridData()
        {
            _gridData.PrintGridState();
        }


        private void Start()
        {
            StopPlacement();
            
            _gridData = new GridData(gridManager.GridSize);
            _buildingPlacer = new BuildingPlacer(_gridData, gridManager.GridSize);

            if (jsonFile != null)
            {
                PlaceObjectsFromJson(jsonFile.text);
            }

            inputManager.OnExit += StopPlacement;
        }

        private void PlaceObjectsFromJson(string json)
        {
            var gridObjectsContainer = JsonUtility.FromJson<GridObjectContainer>(json);

            var gridObjects = new List<GridObjectData>();

            foreach (var gridObjectSerializable in gridObjectsContainer.gridObjects)
            {
                var config =
                    buildingsDataBase.buildingConfigs.FirstOrDefault(b =>
                        b.ID == gridObjectSerializable.buildingConfigID);
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

            foreach (var gridObject in gridObjects)
            {
                _buildingPlacer.PlaceBuilding(gridObject.buildingConfig, gridObject.position, buildingsFromJsonContainer);
            }
        }
        
        #region Placement Actions
        public void StartPlacement(BasicBuildingConfig buildingConfig)
        {
            if (currentPlacementMode == PlacementMode.Building)
            {
                StopPlacement();
                currentPlacementMode = PlacementMode.None;
                return;
            }

            currentPlacementMode = PlacementMode.Building;
            
            StopPlacement();
            
            OnChangeGridVisualizationVisibility?.Invoke(true);
            buildingPrefab.Initialize(buildingConfig);

            _buildingState = new StateOfObjectPlacing(resourcesManager, buildingPrefab, gridManager.GridLayout, buildingPreview
                , _gridData, buildingManager, soundFeedback);

            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }
        
        private void StopPlacement()
        {
            if (_buildingState == null)
            {
                return;
            }

            OnChangeGridVisualizationVisibility?.Invoke(false);
            _buildingState.EndState();
            inputManager.OnClicked -= PlaceStructure;
            inputManager.OnExit -= StopPlacement;
            _lastDetectedPosition = Vector3Int.zero;
            _buildingState = null;
            currentPlacementMode = PlacementMode.None;
        }
        #endregion

        public void StartRemoving()
        {
            if (currentPlacementMode == PlacementMode.Removing)
            {
                StopPlacement();
                currentPlacementMode = PlacementMode.None;
                return;
            }
            
            StopPlacement();
            currentPlacementMode = PlacementMode.Removing;
            
            OnChangeGridVisualizationVisibility?.Invoke(true);

            _buildingState = new StateOfObjectRemoving(resourcesManager, buildingManager, gridManager.GridLayout, buildingPreview,
                _gridData, soundFeedback);

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
            Vector3Int gridPosition = gridManager.GridLayout.WorldToCell(mousePosition);

            _buildingState.OnAction(gridPosition);
            StopPlacement();
        }
        private void Update()
        {
            if (_buildingState == null)
            {
                return;
            }

            Vector3 mousePosition = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = gridManager.GridLayout.WorldToCell(mousePosition);
            if (_lastDetectedPosition != gridPosition)
            {
                _buildingState.UpdateState(gridPosition);
                _lastDetectedPosition = gridPosition;
            }
        }
    }

    [Serializable]
    public class GridDataJson
    {
        public Vector2IntJson gridSize;
    }

    [Serializable]
    public class Vector2IntJson
    {
        public int x;
        public int y;
    }

    [Serializable]
    public class GridObjectContainer
    {
        public List<GridObjectSerializableData> gridObjects;
    }
}