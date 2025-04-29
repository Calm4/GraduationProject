    using System;
    using UnityEngine;

    namespace App.Scripts.TurnsBasedSystem
    {
        public class GamePhaseManager : MonoBehaviour
        {
            [SerializeField] private GamePhase _gamePhase;

            [SerializeField] private float waitingToStartTimer;
            [SerializeField] private float constCountDownToStartTimer;
            private float countDownToStartTimer;
            [SerializeField] private float gamePlayingTimer;

            public event Action<GamePhase> OnGameStateChanges;


            private void Awake()
            {
                _gamePhase = GamePhase.Construction;
                countDownToStartTimer = constCountDownToStartTimer;
            }

            private void ChangeGameState(GamePhase phase)
            {
                _gamePhase = phase;
            }

            private void Update()
            {
                switch (_gamePhase)
                {
                    case GamePhase.CountDownToStart:
                        countDownToStartTimer -= Time.deltaTime;
                        if (countDownToStartTimer < 0f)
                        {
                            _gamePhase = GamePhase.Defense;
                            OnGameStateChanges?.Invoke(_gamePhase);
                        }

                        break;
                    case GamePhase.Defense:
                        // больше не переключаем фазу по таймеру,
                        // дождёмся WavesManager и колбэка onAllDone
                        break;
                    
                    case GamePhase.Construction:
                        countDownToStartTimer = constCountDownToStartTimer;
                        gamePlayingTimer = 5f;
                        break;
                }

                Debug.Log(_gamePhase);
            }

            public float GetCountdownToStartTimer()
            {
                return countDownToStartTimer;
            }

            public GamePhase GetCurrentGameState()
            {
                return _gamePhase;
            }

            public void SetCurrentGameState(GamePhase gamePhase) {
                _gamePhase = gamePhase;
                OnGameStateChanges?.Invoke(_gamePhase);
            }

        }
    }