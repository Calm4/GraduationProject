using App.Scripts.Grid;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private PlacementManager placementManager;
        
        [Title("Grid")]
        [field: SerializeField]
        [VerticalGroup("Grid Settings")]
        public GridLayout GridLayout { get; private set; }

        [SerializeField] [VerticalGroup("Grid Settings")]
        private Vector2Int gridSize;

        [SerializeField] [VerticalGroup("Grid Settings")]
        private GameObject gridVisualization;

        [SerializeField] [VerticalGroup("Grid Settings")]
        private GameObject background;

        public GridData GridData { get; private set; }
        
        private void Awake()
        {
            placementManager.OnChangeGridVisualizationVisibility += SetGridVisualizationVisibility;
            GridData = new GridData(GetGridSize());
        }

        private void Start()
        {
            GridSetup();
        }

        [Button, VerticalGroup("Grid Settings")]
        private void GridSetup()
        {
            var gridOffset = new Vector3(-(float)gridSize.x / 2, 0, -(float)gridSize.y / 2);
            GridLayout.transform.position = gridOffset;

            var mapSize = new Vector3((float)gridSize.x / 10, 1, (float)gridSize.y / 10);
            gridVisualization.transform.localScale = mapSize;
            background.transform.localScale = mapSize;
        }

        public Vector2Int GetGridSize()
        {
            return gridSize;
        }
        public void SetGridSize(Vector2Int size)
        {
            gridSize = size;
        }

        private void SetGridVisualizationVisibility(bool visibilityState)
        {
            gridVisualization.SetActive(visibilityState);
        }
    }
}