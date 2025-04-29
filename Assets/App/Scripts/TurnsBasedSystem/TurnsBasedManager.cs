using System;
using App.Scripts.TurnsBasedSystem.Waves;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace App.Scripts.TurnsBasedSystem
{
    public class TurnsBasedManager : MonoBehaviour {
        [Inject] private WavesManager wavesManager;
        [Inject] private GamePhaseManager phaseManager;
    
        private void Start() {
            wavesManager.OnWaveCompleted += waveIndex => {
                phaseManager.SetCurrentGameState(GamePhase.Construction);
            };
        }
    }

}
