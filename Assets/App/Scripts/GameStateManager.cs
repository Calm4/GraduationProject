using App.Scripts;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private PhaseChangerUIPanel _phaseChangerUIPanel;
    private GameState gameState;

    [SerializeField] private float waitingToStartTimer;
    [SerializeField] private float countDownToStartTimer;
    [SerializeField] private float gamePlayingTimer;

    private void Awake()
    {
        _phaseChangerUIPanel.OnPhaseChangerButtonClick += ChangeGameState;
        gameState = GameState.WaitingToStartWave;
    }

    private void ChangeGameState(GameState state)
    {
        gameState = state;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.WaitingToStartWave:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f)
                {
                    gameState = GameState.CountDownToStart;
                }

                break;
            case GameState.CountDownToStart:
                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer < 0f)
                {
                    gameState = GameState.Defense;
                }

                break;
            case GameState.Defense:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    gameState = GameState.Construction;
                }

                break;
            case GameState.Construction:
                Debug.Log("CONSTRUCTION");
                break;
        }
        Debug.Log(gameState);
    }

    public bool IsGamePlaying()
    {
        return gameState == GameState.Defense;
    }

    public bool IsGameCountdownState()
    {
        return gameState == GameState.CountDownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countDownToStartTimer;
    }
}