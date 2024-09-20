using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Префаб врага
    public float spawnInterval = 2f; // Интервал спавна врагов
    public Transform spawnerPoint; // Точка спавна врагов
    public List<Vector2> path; // Путь, который враги будут проходить

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    // Корутин для спавна врагов
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval); // Ожидание перед следующим спавном
        }
    }

    // Метод для спавна врага
    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnerPoint.position, Quaternion.identity);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.SetPath(path);
        }
    }
}