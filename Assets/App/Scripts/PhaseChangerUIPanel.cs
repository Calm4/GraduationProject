using System;
using App.Scripts;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PhaseChangerUIPanel : MonoBehaviour
{
    [Title("Managers")]
    [SerializeField] private GameStateManager gameStateManager;
    
    [Title("Change Button Components")]
    [SerializeField] private Button phaseChangerButton;
    [SerializeField] private Image phaseChangerImage;
    [SerializeField] private TMP_Text phaseChangerButtonTextField;
    [SerializeField] private Sprite defenseImageVariant;
    [SerializeField] private Sprite constructionImageVariant;

    [Title("Countdown Components")]
    [SerializeField] private RectTransform countdownPanel;
    [SerializeField] private TMP_Text countdownTextField;

    private readonly string constructionPhaseText = "Construction";
    private readonly string defensePhaseText = "Defense";
    
    private bool isPressed;
    private GameState _gameState;
    public event Action<GameState> OnPhaseChangerButtonClick;

    private float timer;

    private void Awake()
    {
        gameStateManager.OnDefenseStateEnd += StartConstructionPhase;
    }

    private void Start()
    {
        HideCountdown();
    }

    private void Update()
    {
        SetCurrentGamePhase();
    }

    private void SetCurrentGamePhase()
    {
        timer = gameStateManager.GetCountdownToStartTimer();
        if (timer <= 0)
        {
            countdownTextField.text = "The Defense Begins";
            StartDefensePhase();
        }

        /*if (gameStateManager.GetCurrentGameState() == GameState.WaitingToStartWave)
            return;

        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");*/
        countdownTextField.text = Mathf.Ceil(timer).ToString();
        //StartConstructionPhase();
    }

    private void StartDefensePhase()
    {
        phaseChangerButton.interactable = false;
        HideCountdown();
        SetGamePhasePanelElements(defenseImageVariant, defensePhaseText);
    }

    private void StartConstructionPhase()
    {
        phaseChangerButton.interactable = true;
        isPressed = !isPressed;
        SetGamePhasePanelElements(constructionImageVariant, constructionPhaseText);
    }

    public void PhaseButtonClick()
    {
        Debug.Log("CLICK!!!");
        isPressed = !isPressed;
        if (isPressed)
        {
            ShowCountdown();
            SetGamePhasePanelElements(defenseImageVariant, defensePhaseText);
            OnPhaseChangerButtonClick?.Invoke(GameState.CountDownToStart);
        }
        else
        {
            HideCountdown();
            SetGamePhasePanelElements(constructionImageVariant, constructionPhaseText);
            OnPhaseChangerButtonClick?.Invoke(GameState.Construction);
        }
    }

    private void SetGamePhasePanelElements(Sprite phaseSprite, string phaseName)
    {
        phaseChangerImage.sprite = phaseSprite;
        phaseChangerButtonTextField.text = phaseName;
    }

    private void ShowCountdown()
    {
        countdownPanel.gameObject.SetActive(true);
    }

    private void HideCountdown()
    {
        countdownPanel.gameObject.SetActive(false);
    }
}