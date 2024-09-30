using System;
using App.Scripts;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private PhaseChangerUIPanel _phaseChangerUIPanel;
    private GameState gameState;

    [SerializeField] private float waitingToStartTimer;
    [SerializeField] private float constCountDownToStartTimer;
    private float countDownToStartTimer;
    [SerializeField] private float gamePlayingTimer;

    public event Action OnDefenseStateEnd;
    
    private void Awake()
    {
        _phaseChangerUIPanel.OnPhaseChangerButtonClick += ChangeGameState;
        gameState = GameState.Construction;
        countDownToStartTimer = constCountDownToStartTimer;
    }

    private void ChangeGameState(GameState state)
    {
        gameState = state;
    }

    private void Update()
    {
        switch (gameState)
        {
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
                    countDownToStartTimer = constCountDownToStartTimer;
                    Debug.Log("HUYAAAA");
                    OnDefenseStateEnd?.Invoke();
                    gameState = GameState.Construction;
                }

                break;
            case GameState.Construction:
                countDownToStartTimer = constCountDownToStartTimer;
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

    public GameState GetCurrentGameState()
    {
        return gameState;
    }
}