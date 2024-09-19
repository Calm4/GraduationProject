using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Placement.JsonClasses;
using UnityEngine;

namespace App.Scripts.Placement.Path
{
    public class PathfindingWithJson
    {
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

        public static Dictionary<Vector2, int> LoadGridFromJson(GridObjectContainer gridObjectContainer)
        {
            Dictionary<Vector2, int> grid = new Dictionary<Vector2, int>();

            foreach (var gridObject in gridObjectContainer.gridObjects)
            {
                Vector2 position = new Vector2(gridObject.position.x, gridObject.position.z);
                grid[position] = gridObject.buildingConfigID;
            }

            return grid;
        }

        public static List<Vector2> GeneratePath(Dictionary<Vector2, int> grid, BasicBuildingConfig pathwayConfig, BasicBuildingConfig castleConfig, Vector2 spawnerPosition, Vector2 castlePosition)
        {
            List<Vector2> fullPath = new List<Vector2>();
            Queue<Vector2> queue = new Queue<Vector2>();
            HashSet<Vector2> visited = new HashSet<Vector2>();

            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            queue.Enqueue(spawnerPosition);
            visited.Add(spawnerPosition);

            while (queue.Count > 0)
            {
                Vector2 current = queue.Dequeue();
                fullPath.Add(current);

                if (current == castlePosition)
                {
                    fullPath.Add(castlePosition);
                    break;
                }

                foreach (Vector2 direction in directions)
                {
                    Vector2 neighbor = current + direction;

                    if (grid.ContainsKey(neighbor) && (grid[neighbor] == pathwayConfig.ID) && !visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }

            if (!fullPath.Contains(castlePosition))
            { 
                var halfOfCastleSize = new Vector2(castleConfig.size.x, castleConfig.size.y) / 2 - new Vector2(0.5f, 0.5f);
                fullPath.Add(castlePosition + halfOfCastleSize);
            }

            Debug.Log("Full path length: " + fullPath.Count);
            return OptimizePath(fullPath);
        }

        private static List<Vector2> OptimizePath(List<Vector2> fullPath)
        {
            if (fullPath.Count < 2) return fullPath;

            List<Vector2> optimizedPath = new List<Vector2>();
            optimizedPath.Add(fullPath[0]);

            for (int i = 1; i < fullPath.Count - 1; i++)
            {
                Vector2 prev = fullPath[i - 1];
                Vector2 current = fullPath[i];
                Vector2 next = fullPath[i + 1];

                // Если текущая точка не лежит на одной прямой линии с предыдущей и следующей
                if (!IsOnStraightLine(prev, current, next))
                {
                    optimizedPath.Add(current);
                }
            }

            optimizedPath.Add(fullPath[fullPath.Count - 1]);
            return optimizedPath;
        }

        // Метод проверки, лежат ли три точки на одной прямой
        private static bool IsOnStraightLine(Vector2 a, Vector2 b, Vector2 c)
        {
            return (a.x == b.x && b.x == c.x) || (a.y == b.y && b.y == c.y);
        }
    }
}