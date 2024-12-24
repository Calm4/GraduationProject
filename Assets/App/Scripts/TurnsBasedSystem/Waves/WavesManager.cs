using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.TurnsBasedSystem.Waves
{
    public class WavesManager : MonoBehaviour
    {
        [Title("Managers")] 
        [SerializeField] private TurnsBasedManager turnsBasedManager;
        [SerializeField] private int currentWave;
    }
}
