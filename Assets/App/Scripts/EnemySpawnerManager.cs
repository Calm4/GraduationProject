using System;
using System.Collections;
using System.Collections.Generic;
using App.Scripts;
using App.Scripts.JsonClasses.Path;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Transform spawnerPoint;
    [field: SerializeField] public List<Vector2> Path { get; set; }

    private void Awake()
    {
        
    }

    private void GamePhase_OnGamePhaseStateChange(GamePhase gamePhase)
    {
        if (gamePhase != GamePhase.Defense)
            return;

        StartCoroutine(SpawnEnemies());
    }

    private void Start()
    {
        Debug.Log("Path length: " + Path.Count);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
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
        GameObject enemy = Instantiate(enemyPrefab, spawnerPoint.position, Quaternion.identity);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.SetPath(Path);
        }
    }

    private void OnDrawGizmos()
    {
        if (Path == null || Path.Count == 0)
            return;

        Gizmos.color = Color.green;
        
        // Loop through the Path and draw spheres and lines
        for (int i = 0; i < Path.Count; i++)
        {
            // Convert Vector2 to Vector3, with Y = 0.5f for visibility
            Vector3 point = new Vector3(Path[i].x, 0.5f, Path[i].y);
            
            // Draw a small sphere at each point
            Gizmos.DrawSphere(point, 0.2f);

            // Draw lines between the points to visualize the path
            if (i < Path.Count - 1)
            {
                Vector3 nextPoint = new Vector3(Path[i + 1].x, 0.5f, Path[i + 1].y);
                Gizmos.DrawLine(point, nextPoint);
            }
        }
    }
}