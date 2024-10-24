using System;
using App.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PhaseChangerUIPanel : MonoBehaviour
{
    [SerializeField] private GamePhaseManager gamePhaseManager;
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
    private GamePhase _gamePhase;

    private CountdownHandler countdownHandler;
    private PhaseStateHandler phaseStateHandler;

    private void Awake()
    {
        gamePhaseManager.OnGameStateChanges += GetCurrentGamePhase;
        countdownHandler = new CountdownHandler(countdownPanel, countdownTextField, gamePhaseManager);
        phaseStateHandler = new PhaseStateHandler(phaseChangerButton, phaseChangerImage, phaseChangerButtonTextField);
    }

    private void GetCurrentGamePhase(GamePhase gamePhase)
    {
        _gamePhase = gamePhase;
        StatesController();
    }

    private void StatesController()
    {
        if (_gamePhase == GamePhase.Construction)
        {
            isConstructionPhase = true;
            phaseStateHandler.StartNewPhase(constructionImageVariant, constructionPhaseText, isConstructionPhase);
        }

        if (_gamePhase == GamePhase.Defense)
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
            gamePhaseManager.SetCurrentGameState(GamePhase.Construction);
        }
        else
        {
            countdownHandler.ShowCountdown();
            countdownHandler.StartCountdown();
            phaseStateHandler.SetGamePhasePanelElements(defenseImageVariant, defensePhaseText);
            gamePhaseManager.SetCurrentGameState(GamePhase.CountDownToStart);
        }
    }
}
