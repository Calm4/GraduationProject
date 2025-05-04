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
    
        [SerializeField] private WaveUIController waveUI;

        private void Start() {
            waveUI.Init(wavesManager);

            wavesManager.OnWaveCompleted += waveIndex => {
                phaseManager.SetCurrentGameState(GamePhase.Construction);
            };
        }
    }

}
