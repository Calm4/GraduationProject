using System;
using System.Collections.Generic;
using App.Scripts.TurnsBasedSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace App.Scripts.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [Inject] private GamePhaseManager _gamePhaseManager;
        [Inject] private ExperienceManager _experienceManager;
        
        [SerializeField] private EnemyConfig enemyConfig;
        private readonly EnemyData _enemyData = new();
        
        public float TraveledDistance { get; private set; }
        private List<Vector2> _pathToFinish;  
        private int _currentPointToMoveIndex;
        

        private void Awake()
        {
            _gamePhaseManager.OnGameStateChanges += TestEnemyMethod;
            _enemyData.InitializeEnemyData(enemyConfig);
        }

        private void TestEnemyMethod(GamePhase obj)
        {
            //TODO: СДЕЛАТЬ ЛОГИКУ ДЛЯ ВРАГОВ ОТНОСИТЕЛЬНО СМЕНЫ ФАЗЫ
            Debug.Log("Current phase: " + obj.ToString());
        }

        public void SetPath(List<Vector2> newPath)
        {
            _pathToFinish = newPath;
            _currentPointToMoveIndex = 0;
            if (_pathToFinish != null && _pathToFinish.Count > 0)
            {
                MoveToNextPoint();
            }
        }

        private void Update()
        {
            if (_pathToFinish == null || _currentPointToMoveIndex >= _pathToFinish.Count)
                return;

            Vector2 targetPosition = _pathToFinish[_currentPointToMoveIndex];
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.z);

            if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
            {
                _currentPointToMoveIndex++;
                if (_currentPointToMoveIndex < _pathToFinish.Count)
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
                    _enemyData.CurrentSpeed * Time.deltaTime);
            }
            UpdateProgress();
        }
    
        private void MoveToNextPoint()
        {
            Vector2 targetPosition = _pathToFinish[_currentPointToMoveIndex];
            //Debug.Log($"Moving to next point: {targetPosition}");
        }
        
        private void UpdateProgress()
        {
            if (_pathToFinish == null || _pathToFinish.Count == 0)
            {
                TraveledDistance = 0f;
                return;
            }

            float total = 0f;
            for (int i = 0; i < _currentPointToMoveIndex - 1; i++)
            {
                total += Vector2.Distance(_pathToFinish[i], _pathToFinish[i + 1]);
            }
            if (_currentPointToMoveIndex > 0 && _currentPointToMoveIndex <= _pathToFinish.Count)
            {
                Vector2 lastPoint = _pathToFinish[_currentPointToMoveIndex - 1];
                Vector2 currentPos2D = new Vector2(transform.position.x, transform.position.z);
                total += Vector2.Distance(lastPoint, currentPos2D);
            }
            TraveledDistance = total;
        }
        
        public void TakeDamage(int damage)
        {
            _enemyData.CurrentHealth -= damage;
            Debug.Log($"{gameObject.name} получил {damage} урона, осталось здоровья: {_enemyData.CurrentHealth}");
            if (_enemyData.CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _experienceManager.AddExperience(enemyConfig.ExperienceForKilling);
            // Здесь можно добавить выдачу опыта, эффекты смерти, и т.д.
            Destroy(gameObject);
        }
    }
}