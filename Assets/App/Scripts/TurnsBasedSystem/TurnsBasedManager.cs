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
        
        
        [field: SerializeField] public GamePhase GamePhase { get; private set; }
        private GamePhase[] _phases;

        [SerializeField] private PhaseChangerUIPanel phaseChangerUIPanel;

        public event Action<GamePhase> OnGamePhaseChange;

        private void Awake()
        {
            phaseChangerUIPanel.OnPhaseChangerButtonClick += StartNextPhase;
        }

        private void Start()
        {
            _phases =(GamePhase[])Enum.GetValues(typeof(GamePhase));
        }
    
        private void StartNextPhase(GameState gameState)
        {
            /*var currentPhaseIndex = Array.IndexOf(_phases, GamePhase);
            currentPhaseIndex = (currentPhaseIndex + 1) % _phases.Length;
        
            GamePhase = _phases[currentPhaseIndex];*//*
            Debug.Log("Current Game Phase: " + gameState);*/
        }
    }
}
