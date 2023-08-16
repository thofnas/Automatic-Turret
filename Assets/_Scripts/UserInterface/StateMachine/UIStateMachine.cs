using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.StateMachine
{
    public class UIStateMachine : MonoBehaviour
    {
        [Header("Play Screen")]
        [SerializeField] private Transform _playScreenUITransform;
        public TextMeshProUGUI HealthText;
        public TextMeshProUGUI CurrentSubWaveCount;
        public TextMeshProUGUI CollectedGearsAmount;
        [Header("Lobby Screen")]
        [SerializeField] private Transform _lobbyScreenUITransform;
        public TextMeshProUGUI CurrentWaveCount;
        public TextMeshProUGUI TotalGearsCount;
        public Button StartWaveButton;
        public Button UpgradeHealthButton;
        public TextMeshProUGUI HealthCurrentLevelText;
        public TextMeshProUGUI HealthNextLevelPriceText;
        public Button UpgradeViewRangeButton;
        public TextMeshProUGUI ViewRangeCurrentLevelText;
        public TextMeshProUGUI ViewRangeNextLevelPriceText;
        public Button UpgradeReloadSpeedButton;
        public TextMeshProUGUI ReloadSpeedCurrentLevelText;
        public TextMeshProUGUI ReloadSpeedNextLevelPriceText;
        public Button UpgradeRotationSpeedButton;
        public TextMeshProUGUI RotationSpeedCurrentLevelText;
        public TextMeshProUGUI RotationSpeedNextLevelPriceText;
        public Button UpgradeDamageButton;
        public TextMeshProUGUI DamageCurrentLevelText;
        public TextMeshProUGUI DamageNextLevelPriceText;
        public Button UpgradeBulletSpeedButton;
        public TextMeshProUGUI BulletSpeedCurrentLevelText;
        public TextMeshProUGUI BulletSpeedNextLevelPriceText;
        [Header("Wave Results Screen")] 
        [SerializeField] private Transform _waveResultsParentScreenUITransform;
        [SerializeField] private Transform _waveWonScreenUITransform;
        [SerializeField] private Transform _waveLostScreenUITransform;
        public Button ReturnToLobbyButton;
        public TextMeshProUGUI WaveWonText;
        public TextMeshProUGUI WaveLostText;

        //get/set
        public Transform PlayScreenUITransform { get => _playScreenUITransform; }
        public Transform LobbyScreenUITransform { get => _lobbyScreenUITransform; }
        public Transform WaveResultsParentScreenUITransform { get => _waveResultsParentScreenUITransform; }
        public Transform WaveWonScreenUITransform { get => _waveWonScreenUITransform; }
        public Transform WaveLostScreenUITransform { get => _waveLostScreenUITransform; }

        // state variables
        private UIStateFactory _states;
        public UIState CurrentState { get; set; }
        
        public void Initialize()
        {
            _states = new UIStateFactory(this);
            
            CurrentState = _states.UILobby();
            
            CurrentState.EnterState();
            
            CurrentState.EnableElement();
        }

        private void Start()
        {
            GameEvents.OnWaveWon.AddListener(GameEvents_Wave_OnWon);
            GameEvents.OnWaveLost.AddListener(GameEvents_Wave_OnLost);
        }

        private void OnDestroy()
        {
            GameEvents.OnWaveWon.RemoveListener(GameEvents_Wave_OnWon);
            GameEvents.OnWaveLost.RemoveListener(GameEvents_Wave_OnLost);
        }

        #region Unity methods
        private void Update() => CurrentState.UpdateState();

        #endregion

        private void GameEvents_Wave_OnWon() => WaveWonScreenUITransform.gameObject.SetActive(true);
        private void GameEvents_Wave_OnLost() => WaveLostScreenUITransform.gameObject.SetActive(true);
    }
}
