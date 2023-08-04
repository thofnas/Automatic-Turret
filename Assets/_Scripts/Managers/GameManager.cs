using System;
using Events;
using Turret.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;
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
            GameEvents.OnWaveLost.AddListener(GameEvents_Wave_OnWon);
        }

        private void OnDestroy() => GameEvents.OnItemPicked.RemoveListener(GameEvents_Item_OnItemPicked);

        private void GameEvents_Item_OnItemPicked() => CollectedGearAmount++;

        private void GameEvents_Wave_OnStarted() => IsPlaying = true;

        private void GameEvents_Wave_OnEnded() => IsPlaying = false;

        private void GameEvents_Wave_OnLost() => CollectedGearAmount = 0;
        
        private void GameEvents_Wave_OnWon()
        {
            TotalGearAmount += CollectedGearAmount;
            CollectedGearAmount = 0;
        }
    }
}
