using System;
using App.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhaseChangerUIPanel : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private Button phaseChangerButton;
    [SerializeField] private Image phaseChangerImage;
    [SerializeField] private TMP_Text phaseChangerButtonTextField;
    [SerializeField] private Sprite defenseImageVariant;
    [SerializeField] private Sprite constructionImageVariant;
    [SerializeField] private RectTransform countdownPanel;
    [SerializeField] private TMP_Text countdownTextField;

    private readonly string constructionPhaseText = "Construction";
    private readonly string defensePhaseText = "Defense";

    private bool isConstructionPhase = true;
    private GameState _gameState;
    public event Action<GameState> OnPhaseChangerButtonClick;

    private CountdownHandler countdownHandler;
    private PhaseStateHandler phaseStateHandler;

    private void Awake()
    {
        gameStateManager.OnGameStateChanges += GetCurrentGameState;
        countdownHandler = new CountdownHandler(countdownPanel, countdownTextField, gameStateManager);
        phaseStateHandler = new PhaseStateHandler(phaseChangerButton, phaseChangerImage, phaseChangerButtonTextField);
    }

    private void GetCurrentGameState(GameState gameState)
    {
        _gameState = gameState;
        StatesController();
    }

    private void StatesController()
    {
        if (_gameState == GameState.Construction)
        {
            isConstructionPhase = true;
            phaseStateHandler.StartNewPhase(constructionImageVariant, constructionPhaseText, isConstructionPhase);
        }

        if (_gameState == GameState.Defense)
        {
            isConstructionPhase = false;
            phaseStateHandler.StartNewPhase(defenseImageVariant, defensePhaseText, isConstructionPhase);
            countdownHandler.HideCountdown();
            countdownHandler.StopCountdown();
        }
    }

    private void Update()
    {
        countdownHandler.UpdateCountdown();
    }

    public void PhaseButtonClick()
    {
        Debug.Log("CLICK!!!");
        //currentPhase = (GamePhase)(((int)currentPhase + 1) % System.Enum.GetValues(typeof(GamePhase)).Length);
        isConstructionPhase = !isConstructionPhase;
        if (isConstructionPhase)
        {
            countdownHandler.HideCountdown();
            countdownHandler.StopCountdown();
            countdownHandler.ResetCountdownState();
            phaseStateHandler.SetGamePhasePanelElements(constructionImageVariant, constructionPhaseText);
            OnPhaseChangerButtonClick?.Invoke(GameState.Construction);
        }
        else
        {
            countdownHandler.ShowCountdown();
            countdownHandler.StartCountdown();
            phaseStateHandler.SetGamePhasePanelElements(defenseImageVariant, defensePhaseText);
            OnPhaseChangerButtonClick?.Invoke(GameState.CountDownToStart);
        }
    }
}
