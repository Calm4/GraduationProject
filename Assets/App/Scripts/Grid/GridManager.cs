using System;
using App.Scripts.JsonClasses.Data;
using App.Scripts.Placement;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace App.Scripts.Grid
{
    public class GridManager : MonoBehaviour
    {
        [Inject] private PlacementManager _placementManager;

        public GridData GridData { get; private set; }
        
        [field: SerializeField] [VerticalGroup("Grid Settings")]
        public GridLayout GridLayout { get; private set; }

        [SerializeField] [VerticalGroup("Grid Settings")]
        private Transform gridVisualization;

        [SerializeField] [VerticalGroup("Grid Settings")]
        private Transform gridBackground;
        

        public event Action OnGridLoadFromJson;
        public event Action OnBuildingsLoadFromJson;

        private void Awake()
        {
            _placementManager.OnChangeGridVisualizationVisibility += SetGridVisualizationVisibility;
        }

        private void Start()
        {
            OnGridLoadFromJson?.Invoke();
            OnBuildingsLoadFromJson?.Invoke();
        }
        
        public void InitializeGridManager(GridSaveDataJson gridDataJson)
        {
            var gridSize = new Vector2Int(gridDataJson.gridSize.x, gridDataJson.gridSize.y);
            GridData = new GridData(gridSize);
            
            AdaptGridVisualizationSize(GridData.GridSize);
        }

        [Button, VerticalGroup("Grid Settings")]
        private void AdaptGridVisualizationSize(Vector2Int gridSize)
        {
            var gridOffset = new Vector3(-(float)gridSize.x / 2, 0, -(float)gridSize.y / 2);
            GridLayout.transform.position = gridOffset;

            var adjustSize = new Vector3((float)gridSize.x / 10, 1, (float)gridSize.y / 10);
            gridVisualization.localScale = adjustSize;
            gridBackground.GetComponent<SpriteRenderer>().size = gridSize;
        }

        private void SetGridVisualizationVisibility(bool visibilityState)
        {
            gridVisualization.gameObject.SetActive(visibilityState);
        }
    }
}