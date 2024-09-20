using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using App.Scripts.JsonClasses.Data;
using UnityEngine;

namespace App.Scripts.JsonClasses.Path
{
    public class PathFindingFromJson
    {
         private readonly TextAsset _jsonFile;
         private readonly BasicBuildingConfig _spawnerConfig;
         private readonly BasicBuildingConfig _castleConfig;
         private readonly BasicBuildingConfig _pathwayConfig;
         private readonly GameObject _tempPrefab;

         private readonly GridManager _gridManager;

        public PathFindingFromJson(GridManager gridManager, TextAsset jsonFile, BasicBuildingConfig spawnerConfig,
            BasicBuildingConfig castleConfig, BasicBuildingConfig pathwayConfig, GameObject tempPrefab)
        {
            _gridManager = gridManager;
            _jsonFile = jsonFile;
            _spawnerConfig = spawnerConfig;
            _castleConfig = castleConfig;
            _pathwayConfig = pathwayConfig;
            _tempPrefab = tempPrefab;
        }

        public void FindPathFromJson()
        {
            GridObjectContainerJson gridObjectContainerJson =
                JsonUtility.FromJson<GridObjectContainerJson>(_jsonFile.text);

            var grid = PathFinding.LoadGridFromJson(gridObjectContainerJson);

            Vector2 spawnerPosition = PathFinding.FindObjectPosition(grid, _spawnerConfig);
            Vector2 castlePosition = PathFinding.FindObjectPosition(grid, _castleConfig);

            if (spawnerPosition == new Vector2Int(-1, -1) || castlePosition == new Vector2Int(-1, -1))
            {
                Debug.LogError("Не удалось найти спавнер или замок в JSON.");
                return;
            }

            Vector2Int offset = _gridManager.GridSize / 2;

            List<Vector2> path =
                PathFinding.GeneratePath(grid, _pathwayConfig, _castleConfig, spawnerPosition, castlePosition);

            Debug.Log("Optimized Path length: " + path.Count);

            foreach (Vector2 point in path)
            {
                Vector3 startPoint = new Vector3(point.x - offset.x, 0, point.y - offset.y);
                Vector3 nextPoint = startPoint;  // Следующая точка пути

                // Если следующая точка существует, рисуем линию
                if (point != path[path.Count - 1])
                {
                    nextPoint = new Vector3(path[path.IndexOf(point) + 1].x - offset.x, 0, path[path.IndexOf(point) + 1].y - offset.y);
                    Debug.DrawLine(startPoint, nextPoint, Color.red, 10f);  // Рисует линию между точками
                }

                Debug.Log($"Path point: {point}");
            }

        }
        
    }
}