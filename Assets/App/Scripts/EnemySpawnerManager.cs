using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class MobSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject mobPrefab; // Префаб моба
        [SerializeField] private List<Vector2> path; // Путь, по которому будут двигаться мобы
        [SerializeField] private float spawnInterval = 5f; // Интервал спавна мобов в секундах

        private void Start()
        {
            StartCoroutine(SpawnMobs());
        }

        // Короутина для спавна мобов через определённые интервалы времени
        private IEnumerator SpawnMobs()
        {
            while (true)
            {
                SpawnMob();
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        // Метод для спавна мобов
        private void SpawnMob()
        {
            // Создаём моба в позиции спавнера
            GameObject mobInstance = Instantiate(mobPrefab, transform.position, Quaternion.identity);

            // Передаём мобу путь для движения
            MobMovement mobMovement = mobInstance.GetComponent<MobMovement>();
            if (mobMovement != null)
            {
                mobMovement.SetPath(path); // Передаём путь в скрипт движения моба
            }
            else
            {
                Debug.LogError("Префаб моба не имеет компонента MobMovement!");
            }
        }

        // Метод для задания пути для мобов
        public void SetPath(List<Vector2> newPath)
        {
            path = newPath;
        }
    }
}