using System;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace Waves.StateMachine
{
    public class WaveStateMachine : MonoBehaviour
    {
        public int CurrentWaveID { get; private set; }
        public int CurrentSubWaveID { get; private set; }
        public int CurrentSubWaveIDMax { get; private set; }
        public bool OnAllEnemiesSpawned { get; private set; }
        public WaveBaseState CurrentState { get; set; }

        [SerializeField] private List<WaveSO> _waves;

        // state variables
        private WaveStateFactory _states;
        
        public void Initialize()
        {
            _states = new WaveStateFactory(this);

            CurrentState = _states.WaitingToStartWave();
            
            CurrentState.EnterState();

            ResetWaveData();
        }

        public WaveSO GetCurrentWaveData() => _waves[CurrentWaveID];

        public SubWave GetCurrentSubWaveData() => GetCurrentWaveData().SubWaves[CurrentSubWaveID];

        #region Unity methods
        private void Update() => CurrentState.UpdateState();

        private void OnEnable()
        {
            GameEvents.OnAllEnemiesSpawned.AddListener(GameEvents_Enemy_OnAllEnemiesSpawned);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Waves_OnWaveEnded);
            GameEvents.OnSubWaveEnded.AddListener(GameEvents_Waves_OnSubWaveEnded);
            GameEvents.OnWaveWon.AddListener(GameEvents_Waves_OnWon);
            GameEvents.OnWaveLost.AddListener(GameEvents_Waves_OnLost);
        }

        private void OnDisable()
        {
            GameEvents.OnAllEnemiesSpawned.RemoveListener(GameEvents_Enemy_OnAllEnemiesSpawned);
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Waves_OnWaveEnded);
            GameEvents.OnSubWaveEnded.RemoveListener(GameEvents_Waves_OnSubWaveEnded);
            GameEvents.OnWaveWon.RemoveListener(GameEvents_Waves_OnWon);
            GameEvents.OnWaveLost.RemoveListener(GameEvents_Waves_OnLost);
        }
        #endregion

        private void ResetWaveData()
        {
            OnAllEnemiesSpawned = false;
            CurrentSubWaveID = 0;
            CurrentSubWaveIDMax = GetCurrentWaveData().SubWaves.Count - 1;
        }

        private void GameEvents_Enemy_OnAllEnemiesSpawned()
        {
            OnAllEnemiesSpawned = true;
        }

        private void GameEvents_Waves_OnSubWaveEnded()
        {
            CurrentSubWaveID++;
            OnAllEnemiesSpawned = false;
        }
        
        private void GameEvents_Waves_OnWaveEnded() => ResetWaveData();

        private void GameEvents_Waves_OnWon() => CurrentWaveID++;
        
        private void GameEvents_Waves_OnLost() => ResetWaveData();
    }
}
