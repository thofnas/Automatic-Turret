using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.StateMachine
{
    public class UIStateMachine : MonoBehaviour
    {
        [Header("Play Screen")]
        [SerializeField] private RectTransform _playScreenUITransform;
        public RectTransform HealthBarBackgroundTransform;
        public Image HealthBarFillImage;
        public RectTransform HealthBarForegroundTransform;
        public TextMeshProUGUI CurrentSubWaveCount;
        public TextMeshProUGUI CollectedGearsAmount;
        [Header("Lobby Screen")]
        [SerializeField] private RectTransform _lobbyScreenUITransform;
        public TextMeshProUGUI CurrentWaveCount;
        public TextMeshProUGUI TotalGearsCount;
        public Button StartWaveButton;
        public Button ResetStatsButton;
        public Button ExitGameButton;
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
        [SerializeField] private RectTransform _waveResultsParentScreenUITransform;
        [SerializeField] private RectTransform _waveWonScreenUITransform;
        [SerializeField] private RectTransform _waveLostScreenUITransform;
        public Button ReturnToLobbyButton;
        public TextMeshProUGUI WaveWonText;
        public TextMeshProUGUI WaveLostText;
        public TextMeshProUGUI CollectedGearsAmountInResults;

        //get/set
        public RectTransform PlayScreenUITransform { get => _playScreenUITransform; }
        public RectTransform LobbyScreenUITransform { get => _lobbyScreenUITransform; }
        public RectTransform WaveResultsParentScreenUITransform { get => _waveResultsParentScreenUITransform; }
        public RectTransform WaveWonScreenUITransform { get => _waveWonScreenUITransform; }
        public RectTransform WaveLostScreenUITransform { get => _waveLostScreenUITransform; }
        
        public float HealthBarOneHPSize { get; private set; }
        public float HealthBarOneHPPosition { get; private set; }
        public float HealthBarBackgroundOneHPSize { get; private set; }

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
            HealthBarOneHPSize = HealthBarForegroundTransform.sizeDelta.x;
            HealthBarOneHPPosition = HealthBarForegroundTransform.anchoredPosition.x;
            HealthBarBackgroundOneHPSize = HealthBarBackgroundTransform.sizeDelta.x;
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
