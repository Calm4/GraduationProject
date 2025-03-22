using System.Collections.Generic;
using App.Scripts.Buildings;
using App.Scripts.Grid;
using UnityEngine;

namespace App.Scripts.JsonClasses.Path
{
    public abstract class PathFinding
    {
        private static Vector2 _gridOffset;
        private static readonly  Vector2 CellOffset = new Vector2(0.5f, 0.5f);
        
        public static List<Vector2> GeneratePath(GridManager gridManager, Dictionary<Vector2, int> grid, 
            Building pathway, Building castle, Vector2 spawnerPosition, Vector2 castlePosition)
        {
            _gridOffset = gridManager.GridData.GridSize / 2;
            
            List<Vector2> fullPath = FindPath(grid, pathway, spawnerPosition, castlePosition);
        
            IncludeCastleInPath(fullPath, castlePosition, castle);

            //Debug.Log("Full path length: " + fullPath.Count);
            return OptimizePath(fullPath);
        }

        private static List<Vector2> FindPath(Dictionary<Vector2, int> grid,Building pathway, Vector2 spawnerPosition, 
            Vector2 castlePosition)
        {
            var fullPath = new List<Vector2>();
            var queue = new Queue<Vector2>();
            var visited = new HashSet<Vector2>();

            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            queue.Enqueue(spawnerPosition);
            visited.Add(spawnerPosition);

            while (queue.Count > 0)
            {
                Vector2 current = queue.Dequeue();
                Vector2 adjustedCurrent = AdjustForGridOffset(current);
                fullPath.Add(adjustedCurrent);

                if (current == castlePosition)
                {
                    fullPath.Add(AdjustForGridOffset(castlePosition));
                    break;
                }

                foreach (Vector2 direction in directions)
                {
                    Vector2 neighbor = current + direction;

                    if (grid.ContainsKey(neighbor) && grid[neighbor] == pathway.BuildingConfig.ID && !visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }

            return fullPath;
        }
        
        private static void IncludeCastleInPath(ICollection<Vector2> fullPath, Vector2 castlePosition, 
            Building castle)
        {
            if (fullPath.Contains(castlePosition)) return;
            

            Vector2 halfOfCastleSize = castle.BuildingConfig.size / 2 - CellOffset;
            fullPath.Add(AdjustForGridOffset(castlePosition) + halfOfCastleSize);
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

        private static Vector2 AdjustForGridOffset(Vector2 position)
        {
            
            return position - _gridOffset + CellOffset;
        }

        private static bool IsOnStraightLine(Vector2 a, Vector2 b, Vector2 c)
        {
            return (Mathf.Approximately(a.x, b.x) && Mathf.Approximately(b.x, c.x)) ||
                   (Mathf.Approximately(a.y, b.y) && Mathf.Approximately(b.y, c.y));
        }

        public static Vector2 FindObjectPosition(Dictionary<Vector2, int> gridObjects, Building building)
        {
            foreach (var gridObject in gridObjects)
            {
                if (gridObject.Value == building.BuildingConfig.ID)
                {
                    return gridObject.Key;
                }
            }

            return new Vector2(-1, -1);
        }
    }
}
