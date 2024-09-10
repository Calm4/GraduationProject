using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Buildings;
using App.Scripts.Grid;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using App.Scripts.Placement.JsonClasses;

namespace App.Scripts.Placement
{
    public class JsonLoaderManager : MonoBehaviour
    {
        [Title("Json File With Buildings Info")] 
        [SerializeField] private TextAsset jsonFile;

        [Title("Buildings Parameters")] 
        [SerializeField] private BuildingsDataBase buildingsDataBase;
        [SerializeField] private Transform buildingsFromJsonContainer;
        private BuildingPlacer _buildingPlacer;
        
        [Title("Grid Parameters")]
        [SerializeField] private GridManager gridManager;
        
        private void Awake()
        {
            gridManager.OnGridLoadFromJson += LoadGridSizeFromJson;
            gridManager.OnBuildingsLoadFromJson += PlaceObjectsFromJson;
        }

        private void LoadGridSizeFromJson()
        {
            GridDataJson gridDataJson = JsonConvert.DeserializeObject<GridDataJson>(jsonFile.text);

            if (gridDataJson != null && gridDataJson.gridSize != null)
            {
                var gridSize = new Vector2Int(gridDataJson.gridSize.x, gridDataJson.gridSize.y);
                
                GridData gridData = new GridData(gridSize);
                
                gridManager.SetGridParameters(gridData, gridSize);
            }
            else
            {
                Debug.LogWarning("No grid size found in JSON, using default size.");
            }
        }
        
        private void PlaceObjectsFromJson()
        {
            _buildingPlacer = new BuildingPlacer(gridManager.GridData);
            
            var gridObjectsContainer = JsonUtility.FromJson<GridObjectContainer>(jsonFile.text);

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