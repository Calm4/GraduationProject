using System;
using App.Scripts.Enemies;
using App.Scripts.TurnsBasedSystem;
using App.Scripts.TurnsBasedSystem.WavesData;
using UnityEngine;
using Zenject;

namespace App.Scripts.TurnsBasedSystem.Waves
{
    public class WavesManager : MonoBehaviour
    {
        [Inject] private GamePhaseManager _phaseMgr;
        //[Inject] private EnemySpawnerManager _spawner;  // больше не инжектим

        [SerializeField] private WavesDatabase _db;
        public event Action<int> OnWaveCompleted;

        private EnemySpawnerManager _spawner;    // будем искать сами
        private int _currentWave = 0;

        public int CurrentWave => _currentWave;
        public int TotalWaves => _db.waves.Count;
        
        private void Awake()
        {
            // подпишемся на смену фазы
            _phaseMgr.OnGameStateChanges += OnPhaseChanged;
            // найдём единственный спавнер в сцене (тот, что JsonLoaderManager создал)
        }

        private void OnPhaseChanged(GamePhase phase)
        {
            if (!_spawner)
            {
                _spawner = FindObjectOfType<EnemySpawnerManager>();
                if (!_spawner) { Debug.LogError("WavesManager: не удалось найти EnemySpawnerManager в сцене!");}
                
            }
            if (phase != GamePhase.Defense) return;

            var config = _db.waves[_currentWave];
            _spawner.StartSpawning(config.spawns, () =>
            {
                OnWaveCompleted?.Invoke(_currentWave);
                _currentWave = (_currentWave + 1) % _db.waves.Count;
                _phaseMgr.SetCurrentGameState(GamePhase.Construction);
            });
        }
    }
}