using CustomEventArgs;
using Events;
using Managers;
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
            GameEvents.OnTotalGearAmountChanged.AddListener(GameEvents_Item_OnTotalGearAmountChanged);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnTurretStatUpgraded.AddListener(GameEvents_Upgrade_OnStatUpg);
        }

        public override void ExitState() {
            Ctx.StartWaveButton.onClick.RemoveAllListeners();
            Ctx.UpgradeHealthButton.onClick.RemoveAllListeners();
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

            UpdateUpgradesUI();
        }
        
        private void UpdateUpgradesUI()
        {
            TurretUpgrades healthUpgrades = UpgradeManager.Instance.GetUpgradesForStat(Stat.AmountOfHealth);
            
            Ctx.TotalGearsCount.text = GameManager.Instance.TotalGearAmount.ToString();

            Ctx.HealthCurrentLevelText.text = healthUpgrades.GetCurrentUpgradeLevel().ToString();

            Ctx.HealthNextLevelPriceText.text = healthUpgrades.TryGetNextUpgrade(out Upgrade upgrade) 
                ? upgrade.UpgradePrice.ToString() 
                : "MAX";
        }

        private void GameEvents_Item_OnTotalGearAmountChanged() => UpdateUI();

        private void GameEvents_Wave_OnWaveStarted() => SwitchState(Factory.UIGame());

        private void GameEvents_Upgrade_OnStatUpg(OnStatUpgradeEventArgs onStatUpgradeEventArgs) => UpdateUpgradesUI();
    }
}
