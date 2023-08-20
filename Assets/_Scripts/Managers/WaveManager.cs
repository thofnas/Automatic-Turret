using System.Collections.Generic;
using Events;
using Item;
using UnityEngine;
using Waves;

namespace Managers
{
    public class WaveManager : Singleton<WaveManager>
    {
        [SerializeField] private List<WaveSO> _waves;
        public int CurrentWaveID { get; private set; }
        public int CurrentSubWaveID { get; private set; }
        public int CurrentSubWaveIDMax { get; private set; }
        public int AmountOfGearsInCurrentWave { get; private set; }

        public void Initialize()
        {
            GameEvents.OnWaveEnded.AddListener(GameEvents_Waves_OnWaveEnded);
            GameEvents.OnSubWaveEnded.AddListener(GameEvents_Waves_OnSubWaveEnded);
            GameEvents.OnWaveWon.AddListener(GameEvents_Waves_OnWon);
            GameEvents.OnWaveLost.AddListener(GameEvents_Waves_OnLost);
            
            if (TryGetCurrentWaveData(out WaveSO waveSO))
                AmountOfGearsInCurrentWave = GetMaxAmountOfGearsFromAWave(waveSO);
        }
        
        private void OnDestroy()
        {
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Waves_OnWaveEnded);
            GameEvents.OnSubWaveEnded.RemoveListener(GameEvents_Waves_OnSubWaveEnded);
            GameEvents.OnWaveWon.RemoveListener(GameEvents_Waves_OnWon);
            GameEvents.OnWaveLost.RemoveListener(GameEvents_Waves_OnLost);
        }
        
        public bool TryGetCurrentWaveData(out WaveSO waveSO)
        {
            waveSO = null;
            if (IsTheLastWave(CurrentWaveID)) return false;
            
            waveSO = _waves[CurrentWaveID];
            return true;
        }

        public SubWave GetCurrentSubWaveData()
        {
            return !TryGetCurrentWaveData(out WaveSO waveSO) 
                ? null 
                : waveSO.SubWaves[CurrentSubWaveID];
        }

        public bool IsTheLastWave(int waveID) => CurrentWaveID >= _waves.Count - 1;
        
        public int GetAmountOfGearsFromAllWaves()
        {
            int totalGears = 0;
            
            _waves.ForEach(wave =>
            {
                totalGears += GetMaxAmountOfGearsFromAWave(wave);
            });

            return totalGears;
        }
        

        public void ResetWaveData()
        {
            if (!TryGetCurrentWaveData(out WaveSO waveSO)) return;
            
            CurrentSubWaveID = 0;
            CurrentSubWaveIDMax = waveSO.SubWaves.Count - 1;
        }
        
        private int GetMaxAmountOfGearsFromAWave(WaveSO wave)
        {
            int total = 0;
            
            wave.SubWaves.ForEach(subWave =>
            {
                subWave.EnemiesData.ForEach(data =>
                {
                    for (int i = 0; i < data.EnemyQuantity; i++)
                    {
                        total += data.EnemyPrefab.GearsToDrop;
                    }
                });
            });

            return total;
        }
        
        
        public bool IsCurrentSubwaveTheLast() => CurrentSubWaveID >= CurrentSubWaveIDMax;

        private void GameEvents_Waves_OnWaveEnded() => ResetWaveData();

        private void GameEvents_Waves_OnSubWaveEnded() => CurrentSubWaveID++;

        private void GameEvents_Waves_OnWon()
        {
            if (!TryGetCurrentWaveData(out WaveSO waveSO)) return;

            CurrentWaveID++;
            AmountOfGearsInCurrentWave = GetMaxAmountOfGearsFromAWave(waveSO);
        }

        private void GameEvents_Waves_OnLost() => ResetWaveData();
    }
}
