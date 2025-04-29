using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.TurnsBasedSystem.WavesData
{
    [CreateAssetMenu(fileName = "WavesDatabase", menuName="Configs/DataBases/WavesDatabase", order = 0)]
    public class WavesDatabase : ScriptableObject {
        public List<WaveConfig> waves;       // список волн
    }
}