using System;
using App.Scripts.Buildings;
using App.Scripts.GameResources;
using App.Scripts.Grid;
using App.Scripts.Input;
using App.Scripts.Placement.States;
using App.Scripts.Sound;
using App.Scripts.TurnsBasedSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace App.Scripts.Placement
{
    public class PlacementManager : MonoBehaviour
    {
        [SerializeField, LabelText(""), Space, Title("Current Placement Mode")]
        private PlacementMode currentPlacementMode = PlacementMode.None;

        [Title("Managers"), Space] 
        [Inject] private GridManager _gridManager;
        [Inject] private InputManager _inputManager;
        [Inject] private BuildingManager _buildingManager;
        [Inject] private ResourcesManager _resourcesManager;
        [Inject] private TurnsBasedManager _turnsBasedManager;
        [Inject] private SoundFeedbackManager _soundFeedbackManager;

        [Title("Buildings"), Space] [SerializeField]
        private BuildingPreview buildingPreview;

        private IBuildingState _buildingState;
        private Vector3Int _lastDetectedPosition = Vector3Int.zero;

        public event Action<bool> OnChangeGridVisualizationVisibility;

        [Button]
        public void GetGridData()
        {
            _gridManager.GridData.PrintGridState();
        }

        private void Start()
        {
            StopPlacement();
            _inputManager.OnExit += StopPlacement;

        }

        #region Placement Actions

        public void StartPlacement(Building building)
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

            _buildingState = new StateOfObjectPlacing(_resourcesManager, _buildingManager, _gridManager, building,
                buildingPreview, _soundFeedbackManager);

            _inputManager.OnClicked += PlaceStructure;
            _inputManager.OnExit += StopPlacement;
        }

        public void StopPlacement()
        {
            if (_buildingState == null)
            {
                return;
            }

            OnChangeGridVisualizationVisibility?.Invoke(false);
            _buildingState.EndState();
            _inputManager.OnClicked -= PlaceStructure;
            _inputManager.OnExit -= StopPlacement;
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

            _buildingState = new StateOfObjectRemoving(_resourcesManager, _buildingManager, _gridManager,
                buildingPreview, _soundFeedbackManager);

            _inputManager.OnClicked += PlaceStructure;
            _inputManager.OnExit += StopPlacement;
        }

        private void PlaceStructure()
        {
            if (_inputManager.IsPointerOverUI())
            {
                return;
            }

            Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = _gridManager.GridLayout.WorldToCell(mousePosition);

            _buildingState.OnAction(gridPosition);
            StopPlacement();
        }

        private void Update()
        {
            if (_buildingState == null)
            {
                return;
            }

            Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = _gridManager.GridLayout.WorldToCell(mousePosition);
            if (_lastDetectedPosition != gridPosition)
            {
                _buildingState.UpdateState(gridPosition);
                _lastDetectedPosition = gridPosition;
            }
        }
    }
}