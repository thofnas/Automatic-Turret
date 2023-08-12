using System;
using CustomEventArgs;
using Events;
using Turret.StateMachine;
using UnityEngine;
using Waves.StateMachine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public const int ENEMY_LAYER = 7;
        public const int GROUND_LAYER = 8;

        [SerializeField] private TurretStateMachine _turretStateMachine;
        [SerializeField] private WaveStateMachine _waveStateMachine;
        [SerializeField] private Transform _groundTransform;
        [SerializeField] private int _startingMoneyAmount;
        
        public TurretStateMachine TurretStateMachine { get => _turretStateMachine; }
        public WaveStateMachine WaveStateMachine { get => _waveStateMachine; }
        public Transform GroundTransform { get => _groundTransform; }
        public bool IsPlaying { get; private set; }
        public int TotalGearAmount
        {
            get => _totalGearAmount;
            private set
            {
                _totalGearAmount = value;
                GameEvents.OnTotalGearAmountChanged.Invoke();
            }
        }
        public int CollectedGearAmount
        {
            get => _collectedGearAmount;
            private set
            {
                _collectedGearAmount = value;
                
                GameEvents.OnCollectedGearAmountChanged.Invoke();
                
                if (IsPlaying) return;
                
                TotalGearAmount += _collectedGearAmount;
                _collectedGearAmount = 0;
            }
        }

        private int _totalGearAmount;
        private int _collectedGearAmount;

        public void Initialize()
        {
            GameEvents.OnItemPicked.AddListener(GameEvents_Item_OnItemPicked);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnStarted);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnEnded);
            GameEvents.OnWaveLost.AddListener(GameEvents_Wave_OnLost);
            GameEvents.OnWaveWon.AddListener(GameEvents_Wave_OnWon);
            GameEvents.OnTurretStatUpgraded.AddListener(GameEvents_Stats_OnUpgrade);
        }

        public void Start()
        {
            TotalGearAmount = _startingMoneyAmount;
        }

        private void OnDestroy()
        {
            GameEvents.OnItemPicked.RemoveListener(GameEvents_Item_OnItemPicked);
            GameEvents.OnWaveStarted.RemoveListener(GameEvents_Wave_OnStarted);
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Wave_OnEnded);
            GameEvents.OnWaveLost.RemoveListener(GameEvents_Wave_OnLost);
            GameEvents.OnWaveWon.RemoveListener(GameEvents_Wave_OnWon);
            GameEvents.OnTurretStatUpgraded.RemoveListener(GameEvents_Stats_OnUpgrade);
        }


        private void GameEvents_Item_OnItemPicked() => CollectedGearAmount++;

        private void GameEvents_Wave_OnStarted() => IsPlaying = true;

        private void GameEvents_Wave_OnEnded()
        {
            TotalGearAmount += CollectedGearAmount;
            CollectedGearAmount = 0;
            IsPlaying = false;
        }

        private void GameEvents_Wave_OnLost() => CollectedGearAmount = 0;
        
        private void GameEvents_Wave_OnWon()
        {

        }

        private void GameEvents_Stats_OnUpgrade(OnStatUpgradeEventArgs upgrade) => TotalGearAmount -= upgrade.Price;
    }
}
