using UnityEngine;
using App.Scripts.TurnsBasedSystem;
using DG.Tweening;
using UnityEngine.UI;

namespace App.Scripts.Buildings.UI
{
    public class BuildingUIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private RectTransform buildingPanel; // Панель с кнопками строительства
        [SerializeField] private Button toggleButton; // Кнопка для сворачивания/разворачивания
        [SerializeField] private float hideOffset = 200f; // Смещение вниз в пикселях
        [SerializeField] private float animationDuration = 0.3f; // Длительность анимации

        private GamePhaseManager _gamePhaseManager;
        private Vector2 _showPosition;
        private Vector2 _hidePosition;
        private bool _isHidden;
        private bool _isConstructionPhase;

        private void Awake()
        {
            _gamePhaseManager = FindObjectOfType<GamePhaseManager>();
            if (_gamePhaseManager == null)
            {
                Debug.LogError("GamePhaseManager not found!");
                return;
            }

            if (buildingPanel == null)
            {
                Debug.LogError("Building Panel not assigned!");
                return;
            }

            if (toggleButton == null)
            {
                Debug.LogError("Toggle Button not assigned!");
                return;
            }

            // Сохраняем начальную позицию панели
            _showPosition = buildingPanel.anchoredPosition;
            _hidePosition = _showPosition - new Vector2(0, hideOffset);

            // Подписываемся на нажатие кнопки
            toggleButton.onClick.AddListener(TogglePanelVisibility);
            Debug.Log("Button listener added");
        }

        private void Start()
        {
            // Инициализируем фазу при старте
            if (_gamePhaseManager != null)
            {
                var currentPhase = _gamePhaseManager.GetCurrentGameState();
                _isConstructionPhase = currentPhase == GamePhase.Construction;
                Debug.Log($"Initial phase: {currentPhase}, IsConstructionPhase: {_isConstructionPhase}");
            }
        }

        private void OnEnable()
        {
            if (_gamePhaseManager != null)
            {
                _gamePhaseManager.OnGameStateChanges += HandleGamePhaseChanged;
            }
        }

        private void OnDisable()
        {
            if (_gamePhaseManager != null)
            {
                _gamePhaseManager.OnGameStateChanges -= HandleGamePhaseChanged;
            }

            if (toggleButton != null)
            {
                toggleButton.onClick.RemoveListener(TogglePanelVisibility);
            }
        }

        private void HandleGamePhaseChanged(GamePhase newPhase)
        {
            Debug.Log($"Game phase changed to: {newPhase}");
            switch (newPhase)
            {
                case GamePhase.Construction:
                    _isConstructionPhase = true;
                    ShowBuildingUI();
                    break;
                case GamePhase.CountDownToStart:
                case GamePhase.Defense:
                    _isConstructionPhase = false;
                    HideBuildingUI();
                    break;
            }
        }

        private void TogglePanelVisibility()
        {
            Debug.Log($"TogglePanelVisibility called. IsConstructionPhase: {_isConstructionPhase}, IsHidden: {_isHidden}");
            
            // Проверяем текущую фазу
            if (_gamePhaseManager != null)
            {
                var currentPhase = _gamePhaseManager.GetCurrentGameState();
                _isConstructionPhase = currentPhase == GamePhase.Construction;
                Debug.Log($"Current phase: {currentPhase}, IsConstructionPhase: {_isConstructionPhase}");
            }

            if (!_isConstructionPhase)
            {
                Debug.Log("Not in construction phase, ignoring toggle");
                return;
            }

            if (_isHidden)
            {
                ShowBuildingUI();
            }
            else
            {
                HideBuildingUI();
            }
        }

        private void ShowBuildingUI()
        {
            Debug.Log("Showing building UI");
            if (_isHidden)
            {
                buildingPanel.DOAnchorPos(_showPosition, animationDuration)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => 
                    {
                        _isHidden = false;
                        Debug.Log("Building UI shown");
                    });
            }
        }

        private void HideBuildingUI()
        {
            Debug.Log("Hiding building UI");
            if (!_isHidden)
            {
                buildingPanel.DOAnchorPos(_hidePosition, animationDuration)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => 
                    {
                        _isHidden = true;
                        Debug.Log("Building UI hidden");
                    });
            }
        }
    }
} 