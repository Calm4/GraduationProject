using System;
using App.Scripts.TurnsBasedSystem.Waves;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace App.Scripts.TurnsBasedSystem
{
    public class TurnsBasedManager : MonoBehaviour
    {
        [Title("Managers")] 
        [Inject] private WavesManager _wavesManager;
        
   
        private GamePhase[] _phases;

        [SerializeField] private PhaseChangerUIPanel phaseChangerUIPanel;

        private void Start()
        {
            _phases =(GamePhase[])Enum.GetValues(typeof(GamePhase));
        }
    
        private void StartNextPhase(GamePhase gamePhase)
        {
            /*var currentPhaseIndex = Array.IndexOf(_phases, GamePhase);
            currentPhaseIndex = (currentPhaseIndex + 1) % _phases.Length;
        
            GamePhase = _phases[currentPhaseIndex];*//*
            Debug.Log("Current Game Phase: " + gameState);*/
        }
    }
}
