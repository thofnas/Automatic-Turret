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
            AmountOfGearsInCurrentWave = GetMaxAmountOfGearsFromAWave(GetCurrentWaveData());
        }
        
        private void OnDestroy()
        {
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Waves_OnWaveEnded);
            GameEvents.OnSubWaveEnded.RemoveListener(GameEvents_Waves_OnSubWaveEnded);
            GameEvents.OnWaveWon.RemoveListener(GameEvents_Waves_OnWon);
            GameEvents.OnWaveLost.RemoveListener(GameEvents_Waves_OnLost);
        }
        
        public WaveSO GetCurrentWaveData() => _waves[CurrentWaveID];

        public SubWave GetCurrentSubWaveData() => GetCurrentWaveData().SubWaves[CurrentSubWaveID];

        public void ResetWaveData()
        {
            CurrentSubWaveID = 0;
            CurrentSubWaveIDMax = GetCurrentWaveData().SubWaves.Count - 1;
        }
        
        private int GetMaxAmountOfGearsFromAWave(WaveSO wave)
        {
            int total = 0;
            
            wave.SubWaves.ForEach(subWave =>
            {
                subWave.EnemiesData.ForEach(data =>
                {
                    total += data.EnemyPrefab.GearsToDrop;
                });
            });

            return total;
        }
        
        private void GameEvents_Waves_OnWaveEnded() => ResetWaveData();

        private void GameEvents_Waves_OnSubWaveEnded()
        {
            CurrentSubWaveID++;
        }

        private void GameEvents_Waves_OnWon()
        {
            CurrentWaveID++;
            AmountOfGearsInCurrentWave = GetMaxAmountOfGearsFromAWave(GetCurrentWaveData());
        }

        private void GameEvents_Waves_OnLost() => ResetWaveData();
    }
}
