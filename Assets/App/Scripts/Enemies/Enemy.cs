using System;
using System.Collections.Generic;
using App.Scripts.TurnsBasedSystem;
using UnityEngine;
using Zenject;

namespace App.Scripts.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [Inject] private GamePhaseManager _gamePhaseManager; 
        
        public float speed = 2f;  
        private List<Vector2> path;  
        private int currentTargetIndex = 0;
        
        private float _progress = 0f;
        public float progress => _progress;
        
        public int maxHealth;
        public int health { get; private set; }


        private void Start()
        {
            _gamePhaseManager.OnGameStateChanges += TestEnemyMethod;
            health = maxHealth;
        }

        private void TestEnemyMethod(GamePhase obj)
        {
            Debug.Log("Current phase: " + obj.ToString());
        }

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
            if (path == null || currentTargetIndex >= path.Count)
                return;

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
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(targetPosition.x, transform.position.y, targetPosition.y),
                    speed * Time.deltaTime);
            }
            UpdateProgress();
        }
    
        private void MoveToNextPoint()
        {
            Vector2 targetPosition = path[currentTargetIndex];
            //Debug.Log($"Moving to next point: {targetPosition}");
        }
        
        private void UpdateProgress()
        {
            if (path == null || path.Count == 0)
            {
                _progress = 0f;
                return;
            }

            float total = 0f;
            for (int i = 0; i < currentTargetIndex - 1; i++)
            {
                total += Vector2.Distance(path[i], path[i + 1]);
            }
            if (currentTargetIndex > 0 && currentTargetIndex <= path.Count)
            {
                Vector2 lastPoint = path[currentTargetIndex - 1];
                Vector2 currentPos2D = new Vector2(transform.position.x, transform.position.z);
                total += Vector2.Distance(lastPoint, currentPos2D);
            }
            _progress = total;
        }
        
        public void TakeDamage(int damage)
        {
            health -= damage;
            Debug.Log($"{gameObject.name} получил {damage} урона, осталось здоровья: {health}");
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // Здесь можно добавить выдачу опыта, эффекты смерти, и т.д.
            Destroy(gameObject);
        }
    }
}