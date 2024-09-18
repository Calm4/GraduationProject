using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class MobMovement : MonoBehaviour
    {
        public float speed = 2f; // Скорость передвижения моба
        private List<Vector3> path;
        private int currentTargetIndex = 0;
        private bool isMoving = false;

        public void SetPath(List<Vector2> pathPoints)
        {
            // Преобразуем список Vector2 в список Vector3, добавляя высоту (y = 0)
            path = new List<Vector3>();
            foreach (var point in pathPoints)
            {
                path.Add(new Vector3(point.x, 0, point.y));
            }
            currentTargetIndex = 0; // Начинаем с первой точки
            isMoving = true; // Активируем движение
        }

        private void Update()
        {
            if (isMoving && path != null && path.Count > 0)
            {
                MoveAlongPath();
            }
        }

        private void MoveAlongPath()
        {
            if (currentTargetIndex < path.Count)
            {
                Vector3 targetPosition = path[currentTargetIndex];
                Vector3 moveDirection = targetPosition - transform.position;
                
                if (moveDirection.magnitude < 0.1f)
                {
                    currentTargetIndex++;
                    if (currentTargetIndex >= path.Count)
                    {
                        isMoving = false; 
                        return;
                    }
                    targetPosition = path[currentTargetIndex];
                }

                transform.position += moveDirection.normalized * speed * Time.deltaTime;
            }
        }
    }
}