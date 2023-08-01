using System;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace Waves.StateMachine
{
    public class WaveStateMachine : MonoBehaviour
    {
        public int EnemiesToSpawnCount
        {
            get => _enemiesToSpawnCount;
            private set
            {
                if (_enemiesToSpawnCount + value < 0)
                    throw new ArgumentOutOfRangeException();

                _enemiesToSpawnCount = value;
            }
        }
        public int CurrentWave { get; protected set; } = 0;
        public Transform EnemySpawnPoint { get => _enemySpawnPoint; }
        public WaveBaseState CurrentState { get; set; }

        [SerializeField] private List<WaveSO> _waves;
        [SerializeField] private Transform _enemySpawnPoint;
        private int _enemiesToSpawnCount;

        // state variables
        private WaveStateFactory _states;
        
        public void Initialize()
        {
            _states = new WaveStateFactory(this);

            EnemiesToSpawnCount = GetCurrentWaveData().EnemiesData[0].EnemyQuantity;
            
            CurrentState = _states.WaitingForPlayer();
            
            CurrentState.EnterState();
            
            GameEvents.OnWaveStateChanged.Invoke(CurrentState.ToString());
        }

        public WaveSO GetCurrentWaveData() => _waves[CurrentWave];

        #region Unity methods
        private void Update() => CurrentState.UpdateState();

        private void OnEnable()
        {
            GameEvents.OnEnemySpawned.AddListener(GameEvents_Enemy_OnEnemySpawned);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Waves_OnWaveEnded);
        }
        private void OnDisable()
        {
            GameEvents.OnEnemySpawned.RemoveListener(GameEvents_Enemy_OnEnemySpawned);
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Waves_OnWaveEnded);
        }
        #endregion
        
        private void GameEvents_Enemy_OnEnemySpawned(Enemy enemy) => EnemiesToSpawnCount--;

        private void GameEvents_Waves_OnWaveEnded() => EnemiesToSpawnCount = _waves[0].EnemiesData[0].EnemyQuantity;
    }
}
