using System;
using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Input;
using App.Scripts.Placement.States;
using App.Scripts.Resources;
using App.Scripts.Sound;
using App.Scripts.TurnsBasedSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class PlacementManager : MonoBehaviour
    {
        [SerializeField, LabelText(""), Space, Title("Current Placement Mode")]
        private PlacementMode currentPlacementMode = PlacementMode.None;

        [Title("Managers"), Space] [SerializeField]
        private InputManager inputManager;

        [SerializeField] private GridManager gridManager;
        [SerializeField] private BuildingManager buildingManager;
        [SerializeField] private ResourcesManager resourcesManager;
        [SerializeField] private TurnsBasedManager turnsBasedManager;

        [Title("Buildings"), Space] 
        [SerializeField] private BuildingPreview buildingPreview;

        [SerializeField] private Building buildingPrefab;
        private IBuildingState _buildingState;

        [Title("Sound"), Space] [SerializeField]
        private SoundFeedback soundFeedback;

        private Vector3Int _lastDetectedPosition = Vector3Int.zero;

        public event Action<bool> OnChangeGridVisualizationVisibility;

        [Button]
        public void GetGridData()
        {
            gridManager.GridData.PrintGridState();
        }


        private void Start()
        {
            StopPlacement();

            inputManager.OnExit += StopPlacement;
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

            _buildingState = new StateOfObjectPlacing(resourcesManager, buildingPrefab, gridManager.GridLayout,
                buildingPreview
                , gridManager.GridData, buildingManager, soundFeedback);

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

            currentPlacementMode = PlacementMode.Removing;

            StopPlacement();

            OnChangeGridVisualizationVisibility?.Invoke(true);

            _buildingState = new StateOfObjectRemoving(resourcesManager, buildingManager, gridManager.GridLayout,
                buildingPreview, gridManager.GridData, soundFeedback);

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
}