using System;
using System.Collections;
using System.Collections.Generic;
using App.Scripts.Factories;
using App.Scripts.TurnsBasedSystem;
using App.Scripts.TurnsBasedSystem.WavesData;
using UnityEngine;
using Zenject;

namespace App.Scripts.Enemies {
    public class EnemySpawnerManager : MonoBehaviour {
        [Inject] private IEnemyFactory _enemyFactory;

        [field: SerializeField] public List<Vector2> Path { get; set; }

        private int _alive;
        private bool _spawning;

        public void StartSpawning(List<EnemySpawnInfo> spawnList, Action onAllDone) {
            StopAllCoroutines();                  // сброс старых, если есть
            StartCoroutine(DoSpawn(spawnList, onAllDone));
        }

        private IEnumerator DoSpawn(List<EnemySpawnInfo> list, Action onAllDone) {
            _spawning = true;
            _alive = 0;

            // 1) Спавним всех врагов по списку
            foreach (var info in list) {
                for (int i = 0; i < info.count; i++) {
                    var enemy = _enemyFactory.Create(info.prefab, null);
                    var spawnOffset = new Vector3(0, 0.25f, 0);
                    enemy.transform.position = transform.position + spawnOffset;
                    enemy.SetPath(Path);
                    enemy.OnDeath += () => _alive--;
                    _alive++;
                    yield return new WaitForSeconds(info.spawnInterval);
                }
            }

            // 2) Ждём, пока пользователь не убьёт всех (_alive станет 0)
            yield return new WaitUntil(() => _alive <= 0);

            _spawning = false;
            onAllDone?.Invoke();
        }

        private void OnDrawGizmos() {
            if (Path == null) return;
            Gizmos.color = Color.green;
            for (int i = 0; i < Path.Count; i++) {
                Vector3 p = new Vector3(Path[i].x, 0.5f, Path[i].y);
                Gizmos.DrawSphere(p, 0.2f);
                if (i < Path.Count - 1) {
                    Vector3 n = new Vector3(Path[i+1].x, 0.5f, Path[i+1].y);
                    Gizmos.DrawLine(p, n);
                }
            }
        }
    }
}
