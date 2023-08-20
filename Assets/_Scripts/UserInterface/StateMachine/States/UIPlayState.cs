using System.Collections.Generic;
using Events;
using Managers;
using Turret;
using UnityEngine;
using Waves;

namespace UserInterface.StateMachine.States
{
    public class UIPlayState : UIState
    {
        public UIPlayState(UIStateMachine context, UIStateFactory uiStateFactory)
            : base(context, uiStateFactory) { }
        
        private const int LEVEL_PROGRESS_MULTIPLIER = 3;

        private float _maxHealth;
        private Vector2 _healthBarSize;
        private int _currentSubWaveIndex;
        private float _waveProgress = 0;
        private readonly List<int> _subWavesProgresses = new();
        private readonly List<int> _subWavesProgressesDefault = new();
        private WaveSO _currentWaveData;

        public override void EnterState()
        {
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnWaveEnded);
            GameEvents.OnSubWaveStarted.AddListener(GameEvents_Wave_OnSubWaveStarted);
            GameEvents.OnSubWaveEnded.AddListener(GameEvents_Wave_OnSubWaveEnded);
            GameEvents.OnCollectedGearAmountChanged.AddListener(GameEvents_Item_OnCollectedGearAmountChanged);
            GameEvents.OnWaveLost.AddListener(GameEvents_Wave_OnLost);
            GameEvents.OnWaveWon.AddListener(GameEvents_Wave_OnWon);
            GameEvents.OnTurretDamaged.AddListener(GameEvents_Turret_OnDamaged);
            GameEvents.OnEnemyDestroyed.AddListener(GameEvents_Enemy_OnDestroyed);
            GameEvents.OnEnemySpawned.AddListener(GameEvents_Enemy_OnSpawned);
            Setup();
        }

        public override void ExitState()
        {
            GameEvents.OnWaveStarted.RemoveListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Wave_OnWaveEnded);
            GameEvents.OnSubWaveStarted.RemoveListener(GameEvents_Wave_OnSubWaveStarted);
            GameEvents.OnSubWaveEnded.RemoveListener(GameEvents_Wave_OnSubWaveEnded);
            GameEvents.OnCollectedGearAmountChanged.RemoveListener(GameEvents_Item_OnCollectedGearAmountChanged);
            GameEvents.OnWaveLost.RemoveListener(GameEvents_Wave_OnLost);
            GameEvents.OnWaveWon.RemoveListener(GameEvents_Wave_OnWon);
            GameEvents.OnTurretDamaged.RemoveListener(GameEvents_Turret_OnDamaged);
            GameEvents.OnEnemyDestroyed.RemoveListener(GameEvents_Enemy_OnDestroyed);
            GameEvents.OnEnemySpawned.RemoveListener(GameEvents_Enemy_OnSpawned);
            Ctx.DestroyAllSubwaveIcons();
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates() { }

        public override void EnableElement() => Ctx.PlayScreenUITransform.gameObject.SetActive(true);
        
        public override void DisableElement() => Ctx.PlayScreenUITransform.gameObject.SetActive(false);

        private void Setup()
        {
            _subWavesProgresses.Clear();
            _subWavesProgressesDefault.Clear();
            
            _maxHealth = UpgradeManager.Instance.GetTurretUpgradedStat(Stat.AmountOfHealth);
            _healthBarSize = new Vector2(_maxHealth * Ctx.HealthBarOneHPSize, Ctx.HealthBarForegroundTransform.sizeDelta.y);

            if (!WaveManager.Instance.TryGetCurrentWaveData(out _currentWaveData)) Debug.LogError("Wave data is null.");
            
            _currentSubWaveIndex = 0;
            
            foreach (SubWave subWave in _currentWaveData.SubWaves)
            {
                int progressAmount = 0;
                    
                subWave.EnemiesData.ForEach(data => progressAmount += data.EnemyQuantity);

                progressAmount *= LEVEL_PROGRESS_MULTIPLIER;
                
                _subWavesProgresses.Add(progressAmount);
                _subWavesProgressesDefault.Add(progressAmount);
            }

            Ctx.SpawnSubwaveIcons(_currentWaveData.SubWaves.Count);
        }

        private void UpdatePlayScreenUI()
        {
            Ctx.CollectedGearsAmount.text = GameManager.Instance.CollectedGearAmount.ToString();

            UpdateHealthBar();
            UpdateWaveProgressBar();
        }

        private void UpdateHealthBar()
        {
            if (Ctx.HealthBarFillImage == null) return;
            
            int health = GameManager.Instance.TurretStateMachine.TurretHealth;
            Ctx.HealthBarForegroundTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _healthBarSize.x);
            Ctx.HealthBarFillImage.fillAmount = Mathf.InverseLerp(0f, _maxHealth, health);
            
            ContentFitterRefresh.RefreshContentFitter(Ctx.HealthBarBackgroundTransform);
        }
        
        private void UpdateWaveProgressBar()
        {
            if (Ctx.WaveProgressBarFillImage == null) return;
                
            _waveProgress = 0;

            for (int i = 0; i < _subWavesProgressesDefault.Count; i++)
            {
                _waveProgress += Mathf.InverseLerp(0f, _subWavesProgressesDefault[i], _subWavesProgresses[i]);
            }
            
            Ctx.WaveProgressBarFillImage.fillAmount = Mathf.InverseLerp(0f, _subWavesProgressesDefault.Count, _waveProgress);
        }
        
        private void IncreaseCurrentSubWaveProgress(int amount)
        {
            if (_subWavesProgresses == null) return;
            if (_currentWaveData == null) return;
            
            amount = Mathf.Abs(amount);
            _subWavesProgresses[_currentSubWaveIndex] -= amount;
            
            Debug.Log(_subWavesProgresses[_currentSubWaveIndex]);
            
            if (_subWavesProgresses[_currentSubWaveIndex] < 0) 
                _subWavesProgresses[_currentSubWaveIndex] = 0;

            UpdateWaveProgressBar();
        }

        private void GameEvents_Wave_OnWaveStarted()
        {
            UpdateHealthBar();
            UpdatePlayScreenUI();
        }

        private void GameEvents_Wave_OnWaveEnded()
        {
            SwitchState(Factory.UILobby());
        }

        private void GameEvents_Wave_OnSubWaveStarted() => UpdatePlayScreenUI();

        private void GameEvents_Wave_OnSubWaveEnded()
        {
            UpdatePlayScreenUI();
            _subWavesProgresses[_currentSubWaveIndex] = 0;
            _currentSubWaveIndex++;
        }

        private void GameEvents_Item_OnCollectedGearAmountChanged(int collectedGearAmount) =>
            Ctx.CollectedGearsAmount.text = collectedGearAmount.ToString();

        private void GameEvents_Wave_OnLost() => SwitchState(Factory.UIResults());

        private void GameEvents_Wave_OnWon() => SwitchState(Factory.UIResults());

        private void GameEvents_Turret_OnDamaged(bool b) => UpdateHealthBar();
    
        private void GameEvents_Enemy_OnSpawned(Enemy.Enemy obj) => IncreaseCurrentSubWaveProgress(1);

        private void GameEvents_Enemy_OnDestroyed(Enemy.Enemy obj) => IncreaseCurrentSubWaveProgress(2);
    }
}
