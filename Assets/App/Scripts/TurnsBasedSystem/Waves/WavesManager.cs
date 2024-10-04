using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.TurnsBasedSystem.Waves
{
    public class WavesManager : MonoBehaviour
    {
        [Title("Managers")] 
        [SerializeField] private TurnsBasedManager turnsBasedManager;
        [SerializeField] private int currentWave;

        private void Awake()
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
}
