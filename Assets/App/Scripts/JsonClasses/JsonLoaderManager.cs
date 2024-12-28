using System.Collections.Generic;
using System.Linq;
using App.Scripts.Buildings;
using App.Scripts.Buildings.UI;
using App.Scripts.Enemies;
using App.Scripts.Grid;
using App.Scripts.JsonClasses.Data;
using App.Scripts.JsonClasses.Path;
using App.Scripts.Placement;
using App.Scripts.TurnsBasedSystem;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace App.Scripts.JsonClasses
{
    public class JsonLoaderManager : MonoBehaviour
    {
        [Title("Json Files Data Base")] [SerializeField] private JsonFilesDataBase jsonFilesDB;

        [Title("Buildings Parameters")] 
        [SerializeField] private BuildingsDataBaseBySectionsSO buildingsDataBaseBySections;
        [SerializeField] private Transform buildingsFromJsonContainer;
        private BuildingPlacer _buildingPlacer;


        [Title("Grid Parameters")] 
        [Inject] private GridManager _gridManager;
        [SerializeField] private Building spawner;
        [SerializeField] private Building castle;
        [SerializeField] private Building pathway;

        private PathFindingFromJson _pathFindingFromJson;
        private List<Vector2> _path;


        private void Awake()
        {
            _gridManager.OnGridLoadFromJson += LoadGridSizeFromJson;
            _gridManager.OnBuildingsLoadFromJson += PlaceObjectsFromJson;
        }

        private void LoadGridSizeFromJson()
        {
            GridSaveDataJson gridDataJson = JsonConvert.DeserializeObject<GridSaveDataJson>(jsonFilesDB.BuildingsJsonFile.text);

            _gridManager.InitializeGridManager(gridDataJson);
            
            if (gridDataJson != null)
            {
                _pathFindingFromJson = new PathFindingFromJson(_gridManager, jsonFilesDB, spawner, castle, pathway);
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

            _buildingPlacer = new BuildingPlacer(_gridManager.GridData);
            foreach (var gridObject in gridObjects)
            {
                _buildingPlacer.InstantiateAndPlaceBuilding(gridObject.Building, _gridManager, gridObject.Position,
                    buildingsFromJsonContainer);
            }
        }

        private void OnDestroy()
        {
            _gridManager.OnGridLoadFromJson -= LoadGridSizeFromJson;
            _gridManager.OnBuildingsLoadFromJson -= PlaceObjectsFromJson;
        }
    }
}