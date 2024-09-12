using System;
using UnityEngine;

namespace App.Scripts.TurnsBasedSystem
{
    public class TurnsBasedManager : MonoBehaviour
    {
        [field: SerializeField] public GamePhases GamePhase { get; private set; }
        private GamePhases[] _phases;

        public event Action OnGamePhaseChange;
        
        private void Start()
        {
            _phases =(GamePhases[])Enum.GetValues(typeof(GamePhases));
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.K))
            {
                StartNextPhase();
                OnGamePhaseChange?.Invoke();
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
