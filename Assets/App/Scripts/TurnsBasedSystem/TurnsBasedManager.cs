using System;
using App.Scripts.TurnsBasedSystem.Waves;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.TurnsBasedSystem
{
    public class TurnsBasedManager : MonoBehaviour
    {
        [Title("Managers")] 
        [SerializeField] private WavesManager wavesManager;
        
        
        [field: SerializeField] public GamePhases GamePhase { get; private set; }
        private GamePhases[] _phases;

        public event Action<GamePhases> OnGamePhaseChange;
        
        private void Start()
        {
            _phases =(GamePhases[])Enum.GetValues(typeof(GamePhases));
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.K))
            {
                StartNextPhase();
                OnGamePhaseChange?.Invoke(GamePhase);
            }
            
        }
    
        private void StartNextPhase()
        {
            var currentPhaseIndex = Array.IndexOf(_phases, GamePhase);
            currentPhaseIndex = (currentPhaseIndex + 1) % _phases.Length;
        
            GamePhase = _phases[currentPhaseIndex];
            Debug.Log("Current Game Phase: " + GamePhase);
        }
    }
}
