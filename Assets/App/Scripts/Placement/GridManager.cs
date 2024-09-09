using System;
using Newtonsoft.Json;
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

        [field: SerializeField]
        [VerticalGroup("Grid Settings")]
        public Vector2Int GridSize { get; private set; }

        [SerializeField] [VerticalGroup("Grid Settings")]
        private GameObject gridVisualization;

        [SerializeField] [VerticalGroup("Grid Settings")]
        private GameObject background;

        [SerializeField] private TextAsset jsonFile;

        private void Awake()
        {
            placementManager.OnChangeGridVisualizationVisibility += SetGridVisualizationVisibility;
        }

        private void Start()
        {
            if (jsonFile != null)
            {
                LoadGridSizeFromJson(jsonFile.text);
            }

            GridSetup();
        }

        [Button, VerticalGroup("Grid Settings")]
        private void GridSetup()
        {
            var gridOffset = new Vector3(-(float)GridSize.x / 2, 0, -(float)GridSize.y / 2);
            GridLayout.transform.position = gridOffset;

            var mapSize = new Vector3((float)GridSize.x / 10, 1, (float)GridSize.y / 10);
            gridVisualization.transform.localScale = mapSize;
            background.transform.localScale = mapSize;
        }

        private void LoadGridSizeFromJson(string jsonString)
        {
            GridDataJson gridDataJson = JsonConvert.DeserializeObject<GridDataJson>(jsonString);

            if (gridDataJson != null && gridDataJson.gridSize != null)
            {
                GridSize = new Vector2Int(gridDataJson.gridSize.x, gridDataJson.gridSize.y);
            }
            else
            {
                Debug.LogWarning("No grid size found in JSON, using default size.");
            }
        }

        private void SetGridVisualizationVisibility(bool visibilityState)
        {
            gridVisualization.SetActive(visibilityState);
        }
    }
}