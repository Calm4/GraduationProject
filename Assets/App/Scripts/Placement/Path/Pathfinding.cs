using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Placement.Path
{
    public class Pathfinding
    {
        public static List<Vector2Int> GeneratePath(Dictionary<Vector2Int, int> grid, Vector2Int spawnerPosition, Vector2Int castlePosition)
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

                // Если нашли замок, прекращаем1
                if (current == castlePosition)
                {
                    break;
                }

                // Проверяем соседние клетки
                foreach (Vector2Int direction in directions)
                {
                    Vector2Int neighbor = current + direction;

                    // Проверяем, что сосед существует в сетке, что это дорога (ID = 4), и клетка ещё не посещена
                    if (grid.ContainsKey(neighbor) && grid[neighbor] == 4 && !visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }

            return path;
        }


        private static bool IsWithinGrid(int[,] grid, Vector2 position)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
        }
    }
}