using Events;
using Managers;
using Turret;
using UnityEngine;

namespace UserInterface.StateMachine.States
{
    public class UIPlayState : UIState
    {
        public UIPlayState(UIStateMachine context, UIStateFactory uiStateFactory)
            : base(context, uiStateFactory) { }

        private float _maxHealth;
        private Vector2 _healthBarSize;

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
            
            _maxHealth = UpgradeManager.Instance.GetTurretUpgradedStat(Stat.AmountOfHealth);
            Debug.Log("max health" + _maxHealth);
            _healthBarSize = new Vector2(_maxHealth * Ctx.HealthBarOneHPSize, Ctx.HealthBarForegroundTransform.sizeDelta.y);

            UpdateHealthBar();
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
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates() { }

        public override void EnableElement() => Ctx.PlayScreenUITransform.gameObject.SetActive(true);
        
        public override void DisableElement() => Ctx.PlayScreenUITransform.gameObject.SetActive(false);

        private void UpdateGameUIText()
        {
            //
            // Ctx.CurrentSubWaveCount.text
            //     = $"{WaveManager.Instance.CurrentSubWaveID + 1} / {WaveManager.Instance.CurrentSubWaveIDMax + 1}";

            Ctx.HealthBarForegroundTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _healthBarSize.x);
            
            Ctx.CollectedGearsAmount.text = GameManager.Instance.CollectedGearAmount.ToString();
        }

        private void UpdateHealthBar()
        {
            if (Ctx.HealthBarFillImage == null) return;
            
            int health = GameManager.Instance.TurretStateMachine.TurretHealth;
            Ctx.HealthBarForegroundTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _maxHealth * Ctx.HealthBarOneHPSize);
            Ctx.HealthBarFillImage.fillAmount = Mathf.InverseLerp(0f, _maxHealth, health);
            
            ContentFitterRefresh.RefreshContentFitter(Ctx.HealthBarBackgroundTransform);
        }

        private void GameEvents_Wave_OnWaveStarted()
        {
            UpdateHealthBar();
            UpdateGameUIText();
        }

        private void GameEvents_Wave_OnWaveEnded()
        {
            UpdateGameUIText();
            SwitchState(Factory.UILobby());
        }

        private void GameEvents_Wave_OnSubWaveStarted() => UpdateGameUIText();

        private void GameEvents_Wave_OnSubWaveEnded() => UpdateGameUIText();
        
        private void GameEvents_Item_OnCollectedGearAmountChanged(int collectedGearAmount) =>
            Ctx.CollectedGearsAmount.text = collectedGearAmount.ToString();

        private void GameEvents_Wave_OnLost() => SwitchState(Factory.UIResults());

        private void GameEvents_Wave_OnWon() => SwitchState(Factory.UIResults());

        private void GameEvents_Turret_OnDamaged() => UpdateHealthBar();
    }
}
