using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.TurnsBasedSystem.WavesData
{
    [CreateAssetMenu(fileName = "WaveConfig_", menuName="Configs/TurnBased/WaveConfig", order = 0)]
    public class WaveConfig : ScriptableObject {
        public List<EnemySpawnInfo> spawns;  // все группы для этой волны
    }
}