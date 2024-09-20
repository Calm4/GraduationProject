using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using App.Scripts.JsonClasses.Data;
using UnityEngine;

namespace App.Scripts.JsonClasses.Path
{
    public abstract class PathFinding
    {
        public static List<Vector2> GeneratePath(GridManager gridManager, Dictionary<Vector2, int> grid, BasicBuildingConfig pathwayConfig,
            BasicBuildingConfig castleConfig, Vector2 spawnerPosition, Vector2 castlePosition)
        {
            Debug.LogWarning(gridManager.GridSize.x + "=======" + gridManager.GridSize.y);
            List<Vector2> fullPath = new List<Vector2>();
            Queue<Vector2> queue = new Queue<Vector2>();
            HashSet<Vector2> visited = new HashSet<Vector2>();

            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            queue.Enqueue(spawnerPosition);
            visited.Add(spawnerPosition);

            while (queue.Count > 0)
            {
                Vector2 current = queue.Dequeue();
        
                // Добавляем смещение для каждой точки
                Vector2 adjustedCurrent = current - gridManager.GridSize / 2;
                fullPath.Add(adjustedCurrent);

                if (current == castlePosition)
                {
                    fullPath.Add(castlePosition - gridManager.GridSize / 2); // Смещаем также замок
                    break;
                }

                foreach (Vector2 direction in directions)
                {
                    Vector2 neighbor = current + direction;

                    if (grid.ContainsKey(neighbor) && (grid[neighbor] == pathwayConfig.ID) &&
                        !visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }

            if (!fullPath.Contains(castlePosition))
            {
                var halfOfCastleSize = new Vector2(castleConfig.size.x, castleConfig.size.y) / 2 - new Vector2(0.5f, 0.5f);
                fullPath.Add(castlePosition + halfOfCastleSize - gridManager.GridSize / 2);
            }

            Debug.Log("Full path length: " + fullPath.Count);
            return OptimizePath(fullPath);
        }


        private static List<Vector2> OptimizePath(List<Vector2> fullPath)
        {
            if (fullPath.Count < 2) return fullPath;

            List<Vector2> optimizedPath = new List<Vector2> { fullPath[0] };

            for (int i = 1; i < fullPath.Count - 1; i++)
            {
                Vector2 prev = fullPath[i - 1];
                Vector2 current = fullPath[i];
                Vector2 next = fullPath[i + 1];

                if (!IsOnStraightLine(prev, current, next))
                {
                    optimizedPath.Add(current);
                }
            }

            optimizedPath.Add(fullPath[fullPath.Count - 1]);
            return optimizedPath;
        }
        
        public static Vector2 FindObjectPosition(Dictionary<Vector2, int> grid, BasicBuildingConfig objectConfig)
        {
            foreach (var item in grid)
            {
                if (item.Value == objectConfig.ID)
                {
                    return item.Key;
                }
            }

            return new Vector2(-1, -1);
        }

        public static Dictionary<Vector2, int> LoadGridFromJson(GridObjectContainerJson gridObjectContainerJson)
        {
            Dictionary<Vector2, int> grid = new Dictionary<Vector2, int>();

            foreach (var gridObject in gridObjectContainerJson.gridObjects)
            {
                Vector2 position = new Vector2(gridObject.position.x, gridObject.position.z);
                grid[position] = gridObject.buildingConfigID;
            }

            return grid;
        }

        

        private static bool IsOnStraightLine(Vector2 a, Vector2 b, Vector2 c)
        {
            return (Mathf.Approximately(a.x, b.x) && Mathf.Approximately(b.x, c.x)) ||
                   (Mathf.Approximately(a.y, b.y) && Mathf.Approximately(b.y, c.y));
        }
    }
}