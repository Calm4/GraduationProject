using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        public float speed = 2f;  
        private List<Vector2> path;  
        private int currentTargetIndex = 0;  

        public void SetPath(List<Vector2> newPath)
        {
            path = newPath;
            currentTargetIndex = 0;
            if (path != null && path.Count > 0)
            {
                MoveToNextPoint();
            }
        }

        private void Update()
        {
            if (path == null || currentTargetIndex >= path.Count) return;

            // Перемещаем врага к следующей точке
            Vector2 targetPosition = path[currentTargetIndex];
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.z);

            if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
            {
                currentTargetIndex++;
                if (currentTargetIndex < path.Count)
                {
                    MoveToNextPoint();
                }
                else
                {
                    Debug.Log("Enemy reached the end of the path!");
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(targetPosition.x, transform.position.y, targetPosition.y), speed * Time.deltaTime);
            }
        }
    
        private void MoveToNextPoint()
        {
            Vector2 targetPosition = path[currentTargetIndex];
            //Debug.Log($"Moving to next point: {targetPosition}");
        }
    }
}