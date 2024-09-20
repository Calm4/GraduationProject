using System.Collections.Generic;
using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using App.Scripts.JsonClasses.Data;
using UnityEngine;

namespace App.Scripts.JsonClasses.Path
{
    public class PathFindingFromJson
    {
        private readonly JsonFilesDataBase _jsonFilesDataBase;
        private readonly Building _spawner;
        private readonly Building _castle;
        private readonly Building _pathway;
        private readonly GridManager _gridManager;


        public PathFindingFromJson(GridManager gridManager, JsonFilesDataBase jsonFilesDataBase, Building spawner,
            Building castle, Building pathway)
        {
            _gridManager = gridManager;
            _jsonFilesDataBase = jsonFilesDataBase;
            _spawner = spawner;
            _castle = castle;
            _pathway = pathway;
        }

        public List<Vector2> FindPathFromJson()
        {
            GridObjectContainerJson gridObjectContainerJson =
                JsonUtility.FromJson<GridObjectContainerJson>(_jsonFilesDataBase.BuildingsJsonFile.text);

            var grid = PathFinding.LoadGridFromJson(gridObjectContainerJson);

            Vector2 spawnerPosition = PathFinding.FindObjectPosition(grid, _spawner.BuildingConfig);
            Vector2 castlePosition = PathFinding.FindObjectPosition(grid, _castle.BuildingConfig);

            if (spawnerPosition == new Vector2Int(-1, -1) || castlePosition == new Vector2Int(-1, -1))
            {
                Debug.LogError("Не удалось найти спавнер или замок в JSON.");
                return null;
            }

            Vector2Int offset = _gridManager.GridSize / 2;

            List<Vector2> path =
                PathFinding.GeneratePath(_gridManager, grid, _pathway.BuildingConfig, _castle.BuildingConfig, spawnerPosition,
                    castlePosition);


            foreach (var p in path)
            {
                Debug.Log("Path Points: [" + p.x + ":" + p.y + "]");
            }

            Debug.Log("Optimized Path length: " + path.Count);


            return path;
        }
    }
}