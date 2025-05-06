using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening; // Не забудьте подключить DOTween
using App.Scripts.TurnsBasedSystem;

namespace App.Scripts.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private GameObject menuPanel; // Ссылка на панель меню
        [SerializeField] private float fadeDuration = 0.5f; // Длительность анимации затухания

        private CanvasGroup _canvasGroup;
        private bool _isFading;

        private void Awake()
        {
            // Получаем или добавляем компонент CanvasGroup
            _canvasGroup = menuPanel.GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = menuPanel.AddComponent<CanvasGroup>();
            }

            // Включаем панель меню при старте игры
            if (menuPanel != null)
            {
                menuPanel.SetActive(true);
                _canvasGroup.alpha = 1f;
            }
        }

        private void Start()
        {
            // Подписываемся на события кнопок
            playButton.onClick.AddListener(OnPlayButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnDestroy()
        {
            // Отписываемся от событий кнопок
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            if (!_isFading)
            {
                StartCoroutine(FadeOut());
            }
        }

        private void OnExitButtonClicked()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        private IEnumerator FadeOut()
        {
            _isFading = true;
            float startTime = Time.time;
            float startAlpha = _canvasGroup.alpha;

            while (Time.time < startTime + fadeDuration)
            {
                float t = (Time.time - startTime) / fadeDuration;
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
                yield return null;
            }

            _canvasGroup.alpha = 0f;
            menuPanel.SetActive(false);
            _isFading = false;
        }
    }
} 