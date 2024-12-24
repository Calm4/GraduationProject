using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App.Scripts.TurnsBasedSystem
{
    public class CountdownHandler
    {
        private RectTransform countdownPanel;
        private TMP_Text countdownTextField;
        private GamePhaseManager _gamePhaseManager;
        private float timer;
        private float previousTimerValue;
        private bool isCountingDown;
        private Tween countdownTween;
        private Color defaultTextColor;

        public CountdownHandler(RectTransform panel, TMP_Text textField, GamePhaseManager manager)
        {
            countdownPanel = panel;
            countdownTextField = textField;
            _gamePhaseManager = manager;
            defaultTextColor = countdownTextField.color;
            ResetCountdownState();
        }

        public void ShowCountdown()
        {
            countdownPanel.gameObject.SetActive(true);
        }

        public void HideCountdown()
        {
            countdownPanel.gameObject.SetActive(false);
        }

        public void StartCountdown()
        {
            isCountingDown = true;
            previousTimerValue = -1f; 
        }

        public void StopCountdown()
        {
            isCountingDown = false;
            if (countdownTween != null && countdownTween.IsActive())
            {
                countdownTween.Kill(); 
            }
        }

        public void ResetCountdownState()
        {
            countdownTextField.transform.localScale = Vector3.one;
            countdownTextField.color = defaultTextColor;
        }

        public void UpdateCountdown()
        {
            if (!isCountingDown) return;

            timer = _gamePhaseManager.GetCountdownToStartTimer();
            if (timer <= 0) return;

            countdownTextField.text = Mathf.Ceil(timer).ToString();

            if (!Mathf.Approximately(Mathf.Ceil(timer), Mathf.Ceil(previousTimerValue)) || Mathf.Approximately(previousTimerValue, -1f))
            {
                AnimateCountdown();
                previousTimerValue = timer;
            }
        }

        private void AnimateCountdown()
        {
            if (countdownTween != null && countdownTween.IsActive())
            {
                countdownTween.Kill();
            }

            Sequence countdownSequence = DOTween.Sequence();
            countdownSequence.Append(countdownTextField.transform.DOScale(1.25f, 0.5f).SetEase(Ease.InOutSine));
            countdownSequence.Append(countdownTextField.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutSine));

            countdownSequence.Join(countdownTextField.DOFade(1f, 0.5f).SetEase(Ease.InOutSine));
            countdownSequence.Join(countdownTextField.DOFade(0.25f, 0.5f).SetEase(Ease.InOutSine));

            countdownSequence.OnComplete(() => countdownTextField.color = defaultTextColor);
        }
    }
}
