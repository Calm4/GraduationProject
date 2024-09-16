using App.Scripts.TurnsBasedSystem;
using Sirenix.OdinInspector;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    [Title("Managers")] [SerializeField] 
    private TurnsBasedManager turnsBasedManager;
    
    [SerializeField] private int currentWave;

    private void Awake()
    {
        turnsBasedManager.OnGamePhaseChange += TurnsBasedManagerOnGamePhaseChange;
    }

    private void TurnsBasedManagerOnGamePhaseChange(GamePhases gamePhase)
    {
        
    }

    private void StartWave()
    {
        currentWave += 1;
        Debug.Log("Wave " + currentWave);
    }

    private void EndWave()
    {
        
    }
}
