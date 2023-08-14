using CustomEventArgs;
using Events;
using Managers;
using TMPro;
using Turret;

namespace UserInterface.StateMachine.States
{
    public class UILobbyState : UIState
    {
        public UILobbyState(UIStateMachine context, UIStateFactory uiStateFactory)
            : base(context, uiStateFactory) { }

        private bool _isUpdated;
        
        public override void EnterState()
        {
            _isUpdated = false;
            Ctx.StartWaveButton.onClick.AddListener(() => UIEvents.OnStartWaveButtonClicked.Invoke());
            Ctx.UpgradeHealthButton.onClick.AddListener(() => UIEvents.OnUpgradeButtonClicked.Invoke(Stat.AmountOfHealth));
            Ctx.UpgradeViewRangeButton.onClick.AddListener(() => UIEvents.OnUpgradeButtonClicked.Invoke(Stat.ViewRange));
            Ctx.UpgradeRotationSpeedButton.onClick.AddListener(() => UIEvents.OnUpgradeButtonClicked.Invoke(Stat.RotationSpeed));
            Ctx.UpgradeReloadSpeedButton.onClick.AddListener(() => UIEvents.OnUpgradeButtonClicked.Invoke(Stat.ReloadSpeed));
            Ctx.UpgradeDamageButton.onClick.AddListener(() => UIEvents.OnUpgradeButtonClicked.Invoke(Stat.BulletDamage));
            GameEvents.OnTotalGearAmountChanged.AddListener(GameEvents_Item_OnTotalGearAmountChanged);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnTurretStatUpgraded.AddListener(GameEvents_Upgrade_OnStatUpg);
        }

        public override void ExitState() {
            Ctx.StartWaveButton.onClick.RemoveAllListeners();
            Ctx.UpgradeHealthButton.onClick.RemoveAllListeners();
            Ctx.UpgradeViewRangeButton.onClick.RemoveAllListeners();
            Ctx.UpgradeRotationSpeedButton.onClick.RemoveAllListeners();
            Ctx.UpgradeReloadSpeedButton.onClick.RemoveAllListeners();
            Ctx.UpgradeDamageButton.onClick.RemoveAllListeners();
            GameEvents.OnTotalGearAmountChanged.RemoveListener(GameEvents_Item_OnTotalGearAmountChanged);
            GameEvents.OnWaveStarted.RemoveListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnTurretStatUpgraded.RemoveListener(GameEvents_Upgrade_OnStatUpg);
        }

        public override void UpdateState()
        {
            CheckSwitchStates();

            if (!_isUpdated && Ctx.LobbyScreenUITransform.gameObject.activeSelf)
            {
                UpdateUI();
                _isUpdated = true;
            }
        }

        public override void CheckSwitchStates() { }
        
        public override void EnableElement() => Ctx.LobbyScreenUITransform.gameObject.SetActive(true);

        public override void DisableElement() => Ctx.LobbyScreenUITransform.gameObject.SetActive(false);

        private void UpdateUI()
        {
            Ctx.CurrentWaveCount.text = GameManager.Instance.WaveStateMachine.CurrentWaveID == 0 
                ? "Tutorial" 
                : GameManager.Instance.WaveStateMachine.CurrentWaveID.ToString();
            
            Ctx.TotalGearsCount.text = GameManager.Instance.TotalGearAmount.ToString();

            UpdateUpgradeUI(Stat.AmountOfHealth, Ctx.HealthCurrentLevelText, Ctx.HealthNextLevelPriceText);
            UpdateUpgradeUI(Stat.ViewRange, Ctx.ViewRangeCurrentLevelText, Ctx.ViewRangeNextLevelPriceText);
            UpdateUpgradeUI(Stat.RotationSpeed, Ctx.RotationSpeedCurrentLevelText, Ctx.RotationSpeedNextLevelPriceText);
            UpdateUpgradeUI(Stat.ReloadSpeed, Ctx.ReloadSpeedCurrentLevelText, Ctx.ReloadSpeedNextLevelPriceText);
            UpdateUpgradeUI(Stat.BulletDamage, Ctx.DamageCurrentLevelText, Ctx.DamageNextLevelPriceText);
        }
        
        private void UpdateUpgradeUI(Stat stat, TMP_Text currentLevel, TMP_Text nextLevelPrice)
        {
            TurretUpgrades upgrades = UpgradeManager.Instance.GetUpgradesForStat(stat);

            currentLevel.text = upgrades.GetCurrentUpgradeLevel().ToString();

            nextLevelPrice.text = upgrades.TryGetNextUpgrade(out Upgrade upgrade) 
                ? upgrade.UpgradePrice.ToString() 
                : "MAX";
        }

        private void GameEvents_Item_OnTotalGearAmountChanged() => UpdateUI();

        private void GameEvents_Wave_OnWaveStarted() => SwitchState(Factory.UIGame());

        private void GameEvents_Upgrade_OnStatUpg(OnStatUpgradeEventArgs onStatUpgradeEventArgs) => UpdateUI();
    }
}
