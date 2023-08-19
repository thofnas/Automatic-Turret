using System;
using System.Collections.Generic;
using Events;
using Managers;
using UnityEngine;

namespace Waves.StateMachine
{
    public class WaveStateMachine : MonoBehaviour
    {

        public bool OnAllEnemiesSpawned { get; private set; }
        public WaveState CurrentState { get; set; }
        
        // state variables
        private WaveStateFactory _states;
        
        #region Unity methods
        private void Start()
        {
            _states = new WaveStateFactory(this);

            CurrentState = _states.WaitingToStartWave();
            
            CurrentState.EnterState();

            WaveManager.Instance.ResetWaveData();
        }
        
        private void Update() => CurrentState.UpdateState();

        private void OnEnable()
        {
            GameEvents.OnAllEnemiesSpawned.AddListener(GameEvents_Enemy_OnAllEnemiesSpawned);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Waves_OnWaveEnded);
            GameEvents.OnSubWaveEnded.AddListener(GameEvents_Waves_OnSubWaveEnded);
            GameEvents.OnWaveLost.AddListener(GameEvents_Waves_OnLost);
        }

        private void OnDisable()
        {
            GameEvents.OnAllEnemiesSpawned.RemoveListener(GameEvents_Enemy_OnAllEnemiesSpawned);
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Waves_OnWaveEnded);
            GameEvents.OnSubWaveEnded.RemoveListener(GameEvents_Waves_OnSubWaveEnded);
            GameEvents.OnWaveLost.RemoveListener(GameEvents_Waves_OnLost);
        }
        #endregion
        
        private void GameEvents_Enemy_OnAllEnemiesSpawned() => OnAllEnemiesSpawned = true;

        private void GameEvents_Waves_OnSubWaveEnded() => OnAllEnemiesSpawned = false;

        private void GameEvents_Waves_OnWaveEnded() => OnAllEnemiesSpawned = false;
        
        private void GameEvents_Waves_OnLost() => OnAllEnemiesSpawned = false;
    }
}
