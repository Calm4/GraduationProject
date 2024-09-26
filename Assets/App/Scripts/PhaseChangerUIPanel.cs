using System;
using App.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhaseChangerUIPanel : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private TMP_Text countdownTextField;
    [SerializeField] private Sprite defenseImage;
    [SerializeField] private Sprite constructionImage;
    [SerializeField] private Image phaseChangerImage;
    [SerializeField] private RectTransform countdownPanel;

    private bool isPressed;
    private GameState _gameState;
    public event Action<GameState> OnPhaseChangerButtonClick;

    private float timer;

    private void Start()
    {
        HideCountdown();
    }

    private void Update()
    {
        timer = gameStateManager.GetCountdownToStartTimer();
        if (timer <= 0)
        {
            countdownTextField.text = "The Defense Begins";
        }
        else
        {
            countdownTextField.text = Mathf.Ceil(timer).ToString();
        }
    }

    public void PhaseButtonClick()
    {
        isPressed = !isPressed;
        if (isPressed)
        {
            ShowCountdown();
            phaseChangerImage.sprite = defenseImage;
            countdownTextField.text = "Defense";
            OnPhaseChangerButtonClick?.Invoke(GameState.CountDownToStart);
        }
        else
        {
            phaseChangerImage.sprite = constructionImage;
            countdownTextField.text = "Construction";
            OnPhaseChangerButtonClick?.Invoke(GameState.CountDownToStart);
        }
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