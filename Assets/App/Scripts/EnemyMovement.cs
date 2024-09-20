using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;  // Скорость передвижения врага
    private List<Vector2> path;  // Путь, по которому будет двигаться враг
    private int currentTargetIndex = 0;  // Текущая цель в пути

    // Метод для установки пути
    public void SetPath(List<Vector2> newPath)
    {
        path = newPath;
        Debug.Log(path.Count + "AHAHAHAAHAHAHAH");
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
                // Когда враг достиг конечной точки (замка), можно добавить логику атаки или уничтожения
                Debug.Log("Enemy reached the end of the path!");
            }
        }
        else
        {
            // Продолжаем движение
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(targetPosition.x, transform.position.y, targetPosition.y), speed * Time.deltaTime);
        }
    }

    // Устанавливаем следующую точку как цель
    private void MoveToNextPoint()
    {
        Vector2 targetPosition = path[currentTargetIndex];
        Debug.Log($"Moving to next point: {targetPosition}");
    }
}