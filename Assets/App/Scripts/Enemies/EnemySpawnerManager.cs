using System.Collections;
using System.Collections.Generic;
using App.Scripts.Factories;
using App.Scripts.TurnsBasedSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace App.Scripts.Enemies
{
    public class EnemySpawnerManager : MonoBehaviour
    {
        [Inject] private readonly IEnemyFactory _enemyFactory;
        [Inject] private GamePhaseManager _gamePhaseManager;  // TODO: ОСТАНОВИЛСЯ ТУТ
        
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private Transform spawnerPoint;
        [field: SerializeField] public List<Vector2> Path { get; set; }
        private int _enemyCounter = 1;
        
        private void GamePhase_OnGamePhaseStateChange(GamePhase gamePhase)
        {
            if (gamePhase != GamePhase.Defense)
                return;

            StartCoroutine(SpawnEnemies());
        }

        private void Start()
        {
            _gamePhaseManager.OnGameStateChanges += ABC;
            Debug.Log("Path length: " + Path.Count);
        }

        private void ABC(GamePhase obj)
        {
            Debug.Log("1`2312323``");
            StartCoroutine(SpawnEnemies());

        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(SpawnEnemies());
            }
        }

        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void SpawnEnemy()
        {
            Enemy enemy = _enemyFactory.Create(enemyPrefab, spawnerPoint);
    
            if (enemy != null)
            {
                enemy.name = $"Enemy_{_enemyCounter}"; // Устанавливаем имя
                _enemyCounter++; // Увеличиваем счетчик
                enemy.SetPath(Path);
            }
        }

        private void OnDrawGizmos()
        {
            if (Path == null || Path.Count == 0)
                return;

            Gizmos.color = Color.green;
        
            for (int i = 0; i < Path.Count; i++)
            {
                Vector3 point = new Vector3(Path[i].x, 0.5f, Path[i].y);
            
                Gizmos.DrawSphere(point, 0.2f);

                if (i < Path.Count - 1)
                {
                    Vector3 nextPoint = new Vector3(Path[i + 1].x, 0.5f, Path[i + 1].y);
                    Gizmos.DrawLine(point, nextPoint);
                }
            }
        }
    }
}