using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Placement.JsonClasses;
using UnityEngine;

namespace App.Scripts.Placement.Path
{
    public class PathfindingWithJson
    {
        public static Vector2Int FindObjectPosition(Dictionary<Vector2Int, int> grid, BasicBuildingConfig objectConfig)
        {
            foreach (var item in grid)
            {
                if (item.Value == objectConfig.ID)
                {
                    return item.Key; 
                }
            }

            return new Vector2Int(-1, -1);
        }
        
        public static Dictionary<Vector2Int, int> LoadGridFromJson(GridObjectContainer gridObjectContainer)
        {
            Dictionary<Vector2Int, int> grid = new Dictionary<Vector2Int, int>();

            foreach (var gridObject in gridObjectContainer.gridObjects)
            {
                Vector2Int position = new Vector2Int(gridObject.position.x, gridObject.position.z);
                grid[position] = gridObject.buildingConfigID;
            }

            return grid;
        }
        public static List<Vector2Int> GeneratePath(Dictionary<Vector2Int, int> grid, BasicBuildingConfig pathwayConfig, Vector2Int spawnerPosition, Vector2Int castlePosition)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            queue.Enqueue(spawnerPosition);
            visited.Add(spawnerPosition);

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                path.Add(current);

                if (current == castlePosition)
                {
                    path.Add(castlePosition);
                    break;
                }

                foreach (Vector2Int direction in directions)
                {
                    Vector2Int neighbor = current + direction;

                    if (grid.ContainsKey(neighbor) && (grid[neighbor] == pathwayConfig.ID || neighbor == castlePosition) && !visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }

            if (!path.Contains(castlePosition))
            {
                path.Add(castlePosition);
            }

            return path;
        }
    
    }
}