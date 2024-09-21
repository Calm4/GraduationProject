using System.Collections.Generic;
using System.Linq;
using App.Scripts.Buildings;
using App.Scripts.Buildings.UI;
using App.Scripts.Grid;
using App.Scripts.JsonClasses.Data;
using App.Scripts.JsonClasses.Path;
using App.Scripts.Placement;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.JsonClasses
{
    public class JsonLoaderManager : MonoBehaviour
    {
        [FormerlySerializedAs("filesDB")] [Title("Json Files Data Base")] [SerializeField]
        private JsonFilesDataBase jsonFilesDB;

        [Title("Buildings Parameters")] 
        [SerializeField] private BuildingsDataBaseBySectionsSO buildingsDataBaseBySections;
        [SerializeField] private Transform buildingsFromJsonContainer;
        private BuildingPlacer _buildingPlacer;


        [Title("Grid Parameters")] 
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Building spawner;
        [SerializeField] private Building castle;
        [SerializeField] private Building pathway;

        private PathFindingFromJson _pathFindingFromJson;
        private List<Vector2> _path;


        private void Awake()
        {
            gridManager.OnGridLoadFromJson += LoadGridSizeFromJson;
            gridManager.OnBuildingsLoadFromJson += PlaceObjectsFromJson;
        }

        private void LoadGridSizeFromJson()
        {
            GridSaveDataJson gridDataJson = JsonConvert.DeserializeObject<GridSaveDataJson>(jsonFilesDB.BuildingsJsonFile.text);

            gridManager.InitializeGridManager(gridDataJson);
            
            if (gridDataJson != null)
            {
                _pathFindingFromJson = new PathFindingFromJson(gridManager, jsonFilesDB, spawner, castle, pathway);
                _path = _pathFindingFromJson.FindPathFromJson();
                spawner.GetComponent<EnemySpawnerManager>().Path = _path;
            }
            else
            {
                Debug.LogWarning("No grid size found in JSON, using default size.");
            }
        }
        
        private void PlaceObjectsFromJson()
        {
            _buildingPlacer = new BuildingPlacer(gridManager.GridData);

            var gridObjectsContainer =
                JsonUtility.FromJson<GridObjectContainerJson>(jsonFilesDB.BuildingsJsonFile.text);
            var gridObjects = new List<GridObjectData>();

            foreach (var gridObjectSerializable in gridObjectsContainer.gridObjects)
            {
                Building building = null;

                foreach (var section in buildingsDataBaseBySections.BuildingsDataBaseBySections)
                {
                    building = section.Value.FirstOrDefault(b =>
                        b.BuildingConfig.ID == gridObjectSerializable.buildingConfigID);
                    if (building != null) break;
                }

                if (building != null)
                {
                    var gridObjectData = new GridObjectData(building, gridObjectSerializable.position);
                    gridObjects.Add(gridObjectData);
                }
                else
                {
                    Debug.LogWarning($"Building config with ID {gridObjectSerializable.buildingConfigID} not found.");
                }
            }

            foreach (var gridObject in gridObjects)
            {
                _buildingPlacer.PlaceBuilding(gridObject.Building, gridManager, gridObject.Position,
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