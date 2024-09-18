using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Placement.JsonClasses;
using App.Scripts.Placement.Path;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class JsonPathfindingManager : MonoBehaviour
    {
        [SerializeField] private TextAsset jsonFile;
        [SerializeField] private BasicBuildingConfig spawnerConfig;
        [SerializeField] private BasicBuildingConfig castleConfig;
        [SerializeField] private BasicBuildingConfig pathwayConfig;
        [SerializeField] private GameObject obje;

        [SerializeField] private GridManager gridManager;

        private void Awake()
        {
            gridManager.OnGridSizeSetup += FindPathFromJson;
        }

        private void FindPathFromJson()
        {
            GridObjectContainer gridObjectContainer = JsonUtility.FromJson<GridObjectContainer>(jsonFile.text);

            var grid = PathfindingWithJson.LoadGridFromJson(gridObjectContainer);

            Vector2 spawnerPosition = PathfindingWithJson.FindObjectPosition(grid, spawnerConfig);
            Vector2 castlePosition = PathfindingWithJson.FindObjectPosition(grid, castleConfig);

            if (spawnerPosition == new Vector2Int(-1, -1) || castlePosition == new Vector2Int(-1, -1))
            {
                Debug.LogError("Не удалось найти спавнер или замок в JSON.");
                return;
            }

            Vector2Int offset = gridManager.GridSize / 2;
            Debug.Log(gridManager.GridSize + "!!!");

            List<Vector2> path = PathfindingWithJson.GeneratePath(grid, pathwayConfig, castleConfig, spawnerPosition, castlePosition);

            Debug.Log($"Path offset: {offset}");
            foreach (Vector2 point in path)
            {
                Debug.Log($"Path point: {point}");
                Instantiate(obje, new Vector3(point.x - offset.x + 0.5f, 0, point.y - offset.y + 0.5f), Quaternion.identity);
            }
        }
    }
}