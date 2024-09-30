using System;
using App.Scripts;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

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

    private bool isPressed;
    private GameState _gameState;
    public event Action<GameState> OnPhaseChangerButtonClick;

    private float timer;
    private float previousTimerValue; // Для отслеживания изменений таймера
    private bool isCountingDown; // Флаг состояния обратного отсчета
    private Tween countdownTween; // Для управления анимацией
    private Color defaultTextColor; // Для сохранения исходного цвета текста

    private void Awake()
    {
        gameStateManager.OnDefenseStateEnd += StartConstructionPhase;
        defaultTextColor = countdownTextField.color; // Сохраняем исходный цвет текста
    }

    private void Start()
    {
        ResetCountdownState(); // Устанавливаем стандартное состояние
        previousTimerValue = -1f; // Инициализация для первой анимации
    }

    private void Update()
    {
        if (isCountingDown)
        {
            SetCurrentGamePhase();
        }
    }

    private void SetCurrentGamePhase()
    {
        timer = gameStateManager.GetCountdownToStartTimer();
        if (timer <= 0)
        {
            countdownTextField.text = "The Defense Begins";
            StartDefensePhase();
            return;
        }

        countdownTextField.text = Mathf.Ceil(timer).ToString();

        // Проверяем, изменилось ли значение таймера или это первый запуск
        if (Mathf.Ceil(timer) != Mathf.Ceil(previousTimerValue) || previousTimerValue == -1f)
        {
            AnimateCountdown();
            previousTimerValue = timer; // Обновляем предыдущее значение таймера
        }
    }

    private void StartDefensePhase()
    {
        phaseChangerButton.interactable = false;
        HideCountdown();
        SetGamePhasePanelElements(defenseImageVariant, defensePhaseText);
        StopCountdown();
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
            StartCountdown();
            SetGamePhasePanelElements(defenseImageVariant, defensePhaseText);
            OnPhaseChangerButtonClick?.Invoke(GameState.CountDownToStart);
        }
        else
        {
            HideCountdown();
            StopCountdown();
            ResetCountdownState(); // Возвращаем всё в исходное состояние
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

    private void StartCountdown()
    {
        isCountingDown = true;
        previousTimerValue = -1f; // Сбрасываем, чтобы первая цифра анимировалась
    }

    private void StopCountdown()
    {
        isCountingDown = false;
        if (countdownTween != null && countdownTween.IsActive())
        {
            countdownTween.Kill(); // Останавливаем анимацию, если она активна
        }
    }

    private void ResetCountdownState()
    {
        countdownTextField.transform.localScale = Vector3.one; // Возвращаем масштаб по умолчанию
        countdownTextField.color = defaultTextColor; // Возвращаем исходную прозрачность (альфа = 1)
    }

    private void AnimateCountdown()
    {
        // Останавливаем текущую анимацию, если она существует
        if (countdownTween != null && countdownTween.IsActive())
        {
            countdownTween.Kill();
        }

        // Создаём последовательность для анимаций масштаба и прозрачности
        Sequence countdownSequence = DOTween.Sequence();

        // Анимация масштаба
        countdownSequence.Append(
            countdownTextField.transform.DOScale(1.25f, 0.5f).SetEase(Ease.InOutSine)
        );

        countdownSequence.Append(
            countdownTextField.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutSine)
        );

        // Привязываем прозрачность к изменению масштаба
        countdownSequence.Join(
            countdownTextField.DOFade(1f, 0.5f).SetEase(Ease.InOutSine)
        );

        countdownSequence.Join(
            countdownTextField.DOFade(0.25f, 0.5f).SetEase(Ease.InOutSine)
        );

        // Восстанавливаем прозрачность в конце
        countdownSequence.OnComplete(() => countdownTextField.color = defaultTextColor);
    }

}
