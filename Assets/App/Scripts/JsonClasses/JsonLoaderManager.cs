using System.Collections.Generic;
using System.Linq;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Buildings.UI;
using App.Scripts.Grid;
using App.Scripts.JsonClasses.Data;
using App.Scripts.JsonClasses.Path;
using App.Scripts.Placement;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.JsonClasses
{
    public class JsonLoaderManager : MonoBehaviour
    {
        [Title("Json File With Buildings Info")] [SerializeField]
        private TextAsset jsonFile;

        [Title("Buildings Parameters")] [SerializeField]
        private BuildingsDataBaseBySectionsSO buildingsDataBaseBySections;

        [SerializeField] private Transform buildingsFromJsonContainer;
        private BuildingPlacer _buildingPlacer;

        [Title("Grid Parameters")] [SerializeField]
        private GridManager gridManager;

        [SerializeField] private BasicBuildingConfig spawnerConfig;
        [SerializeField] private BasicBuildingConfig castleConfig;
        [SerializeField] private BasicBuildingConfig pathwayConfig;
        [SerializeField] private GameObject tempPrefab;

        private PathFindingFromJson pathFindingFromJson;
        
        private void Awake()
        {
            pathFindingFromJson = new PathFindingFromJson(gridManager, jsonFile, spawnerConfig, castleConfig,
                pathwayConfig, tempPrefab);
            gridManager.OnGridLoadFromJson += LoadGridSizeFromJson;
            gridManager.OnBuildingsLoadFromJson += PlaceObjectsFromJson;
        }

        private void LoadGridSizeFromJson()
        {
            GridSaveDataJson gridDataJson = JsonConvert.DeserializeObject<GridSaveDataJson>(jsonFile.text);

            if (gridDataJson != null)
            {
                var gridSize = new Vector2Int(gridDataJson.gridSize.x, gridDataJson.gridSize.y);
                GridData gridData = new GridData(gridSize);
                gridManager.SetGridParameters(gridData, gridSize);
                pathFindingFromJson.FindPathFromJson();
            }
            else
            {
                Debug.LogWarning("No grid size found in JSON, using default size.");
            }
        }

        private void PlaceObjectsFromJson()
        {
            _buildingPlacer = new BuildingPlacer(gridManager.GridData);

            var gridObjectsContainer = JsonUtility.FromJson<GridObjectContainerJson>(jsonFile.text);
            var gridObjects = new List<GridObjectData>();

            foreach (var gridObjectSerializable in gridObjectsContainer.gridObjects)
            {
                BasicBuildingConfig config = null;

                foreach (var section in buildingsDataBaseBySections.BuildingsDataBaseBySections)
                {
                    config = section.Value.FirstOrDefault(b => b.ID == gridObjectSerializable.buildingConfigID);
                    if (config != null) break; 
                }

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
                _buildingPlacer.PlaceBuilding(gridObject.buildingConfig, gridManager, gridObject.position,
                    buildingsFromJsonContainer);
            }
        }

        private void OnDestroy()
        {
            gridManager.OnGridLoadFromJson -= LoadGridSizeFromJson;
            gridManager.OnBuildingsLoadFromJson -= PlaceObjectsFromJson;
        }
    }
}