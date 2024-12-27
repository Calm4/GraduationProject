using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace App.Scripts.TurnsBasedSystem.Waves
{
    public class WavesManager : MonoBehaviour
    {
        [Title("Managers")] 
        [Inject] private TurnsBasedManager _turnsBasedManager;
        [SerializeField] private int currentWave;
    }
}
