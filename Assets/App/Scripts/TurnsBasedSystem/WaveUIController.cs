using TMPro;
using UnityEngine;
using App.Scripts.TurnsBasedSystem.Waves;

public class WaveUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;

    private WavesManager _wavesManager;

    public void Init(WavesManager wavesManager)
    {
        _wavesManager = wavesManager;
        UpdateWaveText(_wavesManager.CurrentWave, _wavesManager.TotalWaves);
        _wavesManager.OnWaveCompleted += OnWaveCompleted;
    }

    private void OnDestroy()
    {
        if (_wavesManager != null)
            _wavesManager.OnWaveCompleted -= OnWaveCompleted;
    }

    private void OnWaveCompleted(int waveIndex)
    {
        int nextWave = (waveIndex + 1);
        UpdateWaveText(nextWave, _wavesManager.TotalWaves);
    }

    private void UpdateWaveText(int currentWave, int totalWaves)
    {
        waveText.text = $"Wave {currentWave + 1} / {totalWaves}";
    }
}