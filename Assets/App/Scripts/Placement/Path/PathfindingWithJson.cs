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
            List<Vector2> path = new List<Vector2>();
            Queue<Vector2> queue = new Queue<Vector2>();
            HashSet<Vector2> visited = new HashSet<Vector2>();

            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            queue.Enqueue(spawnerPosition);
            visited.Add(spawnerPosition);

            while (queue.Count > 0)
            {
                Vector2 current = queue.Dequeue();
                path.Add(current);

                if (current == castlePosition)
                {
                    path.Add(castlePosition);
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

            if (!path.Contains(castlePosition))
            { 
                var halfOfCastleSize = new Vector2(castleConfig.size.x, castleConfig.size.y) / 2 - new Vector2(0.5f, 0.5f);
                path.Add(castlePosition + halfOfCastleSize);
            }

            return path;
        }
    
    }
}