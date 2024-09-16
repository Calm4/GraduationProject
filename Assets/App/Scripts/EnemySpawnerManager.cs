using System;
using System.Collections;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [Range(0, 2)] [SerializeField] private float timeToSpawnNextEnemy;
    [SerializeField] private int enemyCount;
    private float _timer;

    private void Start()
    {
        StartCoroutine(SpawnWaveOfEnemies());
    }

    private void Update()
    {
    }

    private IEnumerator SpawnWaveOfEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            var enemy = Instantiate(enemyPrefab, transform.position + new Vector3(i,0,0), Quaternion.identity);
            yield return new WaitForSeconds(timeToSpawnNextEnemy);
        }
    }
}