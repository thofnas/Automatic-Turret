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
        
        public override void EnterState() {
            Ctx.StartWaveButton.onClick.AddListener(() => UIEvents.OnStartWaveButtonClicked.Invoke());
            Ctx.UpgradeHealthButton.onClick.AddListener(() => UIEvents.OnUpgradeButtonClicked.Invoke(Stat.AmountOfHealth));
            GameEvents.OnTotalGearAmountChanged.AddListener(GameEvents_Item_OnTotalGearAmountChanged);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnWaveEnded);
            GameEvents.OnStatUpgraded.AddListener(GameEvents_Upgrade_OnStatUpg);
            EnableElement();
        }

        public override void ExitState() {
            Ctx.StartWaveButton.onClick.RemoveAllListeners();
            Ctx.UpgradeHealthButton.onClick.RemoveAllListeners();
            GameEvents.OnTotalGearAmountChanged.RemoveListener(GameEvents_Item_OnTotalGearAmountChanged);
            GameEvents.OnWaveStarted.RemoveListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Wave_OnWaveEnded);
            GameEvents.OnStatUpgraded.RemoveListener(GameEvents_Upgrade_OnStatUpg);
        }

        public override void UpdateState() => CheckSwitchStates();

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

        private void GameEvents_Wave_OnWaveEnded() => UpdateUI();

        private void GameEvents_Wave_OnWaveStarted() => SwitchState(Factory.UIGame());

        private void GameEvents_Upgrade_OnStatUpg(OnStatUpgradeEventArgs onStatUpgradeEventArgs) => UpdateUpgradesUI();
    }
}
