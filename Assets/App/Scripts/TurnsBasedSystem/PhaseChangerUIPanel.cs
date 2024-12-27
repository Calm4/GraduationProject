using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.TurnsBasedSystem
{
    public class PhaseChangerUIPanel : MonoBehaviour
    {
        [Inject] private GamePhaseManager _gamePhaseManager;
        
        [SerializeField] private Button phaseChangerButton;
        [SerializeField] private Image phaseChangerImage;
        [SerializeField] private TMP_Text phaseChangerTextField;
        [Space(10)]
        [SerializeField] private RectTransform countdownPanel;
        [SerializeField] private TMP_Text countdownTextField;
        [Space(10)]
        [SerializeField] private Sprite defenseImageVariant;
        [SerializeField] private Sprite constructionImageVariant;

        private readonly string _constructionPhaseText = "Construction";
        private readonly string _defensePhaseText = "Defense";

        private bool _isConstructionPhase = true;
        private GamePhase _gamePhase;

        private CountdownHandler _countdownHandler;
        private PhaseStateHandler _phaseStateHandler;

        private void Awake()
        {
            _gamePhaseManager.OnGameStateChanges += GetCurrentGamePhase;
            _countdownHandler = new CountdownHandler(countdownPanel, countdownTextField, _gamePhaseManager);
            _phaseStateHandler = new PhaseStateHandler(phaseChangerButton, phaseChangerImage, phaseChangerTextField);
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
                _isConstructionPhase = true;
                _phaseStateHandler.StartNewPhase(constructionImageVariant, _constructionPhaseText, _isConstructionPhase);
            }

            if (_gamePhase == GamePhase.Defense)
            {
                _isConstructionPhase = false;
                _phaseStateHandler.StartNewPhase(defenseImageVariant, _defensePhaseText, _isConstructionPhase);
                _countdownHandler.HideCountdown();
                _countdownHandler.StopCountdown();
            }
        }

        private void Update()
        {
            _countdownHandler.UpdateCountdown();
        }

        public void PhaseButtonClick()
        {
            Debug.Log("CLICK!!!");
            //currentPhase = (GamePhase)(((int)currentPhase + 1) % System.Enum.GetValues(typeof(GamePhase)).Length);
            _isConstructionPhase = !_isConstructionPhase;
            if (_isConstructionPhase)
            {
                _countdownHandler.HideCountdown();
                _countdownHandler.StopCountdown();
                _countdownHandler.ResetCountdownState();
                _phaseStateHandler.SetGamePhasePanelElements(constructionImageVariant, _constructionPhaseText);
                _gamePhaseManager.SetCurrentGameState(GamePhase.Construction);
            }
            else
            {
                _countdownHandler.ShowCountdown();
                _countdownHandler.StartCountdown();
                _phaseStateHandler.SetGamePhasePanelElements(defenseImageVariant, _defensePhaseText);
                _gamePhaseManager.SetCurrentGameState(GamePhase.CountDownToStart);
            }
        }
    }
}
