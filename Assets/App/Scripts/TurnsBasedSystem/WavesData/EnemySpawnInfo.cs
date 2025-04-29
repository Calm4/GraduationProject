using System;
using App.Scripts.Enemies;
using UnityEngine;

namespace App.Scripts.TurnsBasedSystem.WavesData
{
    [Serializable]
    public struct EnemySpawnInfo
    {
        public Enemy prefab;      // префаб врага
        public int count;              // сколько заспавнить
        public float spawnInterval;    // задержка между спавнами
    }
}