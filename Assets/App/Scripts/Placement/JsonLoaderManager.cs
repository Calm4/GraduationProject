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
        [Title("Json File With Buildings Info")] [SerializeField]
        private TextAsset jsonFile;

        [Title("Buildings Parameters")] 
        [SerializeField] private BuildingsDataBase buildingsDataBase;
        [SerializeField] private Transform buildingsFromJsonContainer;
        private BuildingPlacer _buildingPlacer;
        
        [Title("Grid Parameters")]
        [SerializeField] private GridManager gridManager;

        private void Start()
        {
            _buildingPlacer = new BuildingPlacer(gridManager.GridData);

            if (jsonFile != null)
            {
                LoadGridSizeFromJson(jsonFile.text);
                PlaceObjectsFromJson(jsonFile.text);
            }
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
                _buildingPlacer.PlaceBuilding(gridObject.buildingConfig, gridManager, gridObject.position,
                    buildingsFromJsonContainer);
            }
        }

        private void LoadGridSizeFromJson(string jsonString)
        {
            GridDataJson gridDataJson = JsonConvert.DeserializeObject<GridDataJson>(jsonString);

            if (gridDataJson != null && gridDataJson.gridSize != null)
            {
                var gridSize = new Vector2Int(gridDataJson.gridSize.x, gridDataJson.gridSize.y);
                gridManager.SetGridSize(gridSize);
                Debug.Log(gridSize);
            }
            else
            {
                Debug.LogWarning("No grid size found in JSON, using default size.");
            }
        }
    }
}