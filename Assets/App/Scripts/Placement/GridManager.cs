using System;
using App.Scripts.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Placement
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private PlacementManager placementManager;
        
        [Title("Grid")]
        [field: SerializeField]
        [VerticalGroup("Grid Settings")]
        public GridLayout GridLayout { get; private set; }

        [field: SerializeField] [VerticalGroup("Grid Settings")]
        public Vector2Int GridSize { get; private set; }

        [SerializeField] [VerticalGroup("Grid Settings")]
        private GameObject gridVisualization;

        [SerializeField] [VerticalGroup("Grid Settings")]
        private GameObject background;

        public GridData GridData { get; private set; }

        public event Action OnGridLoadFromJson; 
        public event Action OnBuildingsLoadFromJson; 
        
        private void Awake()
        {
            placementManager.OnChangeGridVisualizationVisibility += SetGridVisualizationVisibility;
        }

        private void Start()
        {
            OnGridLoadFromJson?.Invoke();
            OnBuildingsLoadFromJson?.Invoke();
            GridSetup();
        }

        [Button, VerticalGroup("Grid Settings")]
        private void GridSetup()
        {
            var gridOffset = new Vector3(-(float)GridSize.x / 2, 0, -(float)GridSize.y / 2);
            GridLayout.transform.position = gridOffset;

            var adjustSize = new Vector3((float)GridSize.x / 10, 1, (float)GridSize.y / 10);
            gridVisualization.transform.localScale = adjustSize;
            background.transform.localScale = adjustSize;
        }

        public void SetGridParameters(GridData gridData, Vector2Int gridSize)
        {
            GridData = gridData;
            GridSize = gridSize;
        }

        private void SetGridVisualizationVisibility(bool visibilityState)
        {
            gridVisualization.SetActive(visibilityState);
        }
    }
}