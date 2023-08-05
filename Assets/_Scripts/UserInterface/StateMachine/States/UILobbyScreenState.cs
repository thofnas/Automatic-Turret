using Events;
using Managers;
using Turret;

namespace UserInterface.StateMachine.States
{
    public class UILobbyScreenState : UIBaseState
    {
        public UILobbyScreenState(UIStateMachine context, UIStateFactory uiStateFactory)
            : base(context, uiStateFactory) { }
        
        public override void EnterState() {
            Ctx.StartWaveButton.onClick.AddListener(() => UIEvents.OnStartWaveButtonClicked.Invoke());
            Ctx.UpgradeHealthButton.onClick.AddListener(() => UIEvents.OnUpgradeButtonClicked.Invoke(Stat.AmountOfHealth));
            GameEvents.OnTotalGearAmountChanged.AddListener(GameEvents_Item_OnTotalGearAmountChanged);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnWaveEnded);
        }

        public override void ExitState() {
            Ctx.StartWaveButton.onClick.RemoveAllListeners();
            Ctx.UpgradeHealthButton.onClick.RemoveAllListeners();
            GameEvents.OnTotalGearAmountChanged.RemoveListener(GameEvents_Item_OnTotalGearAmountChanged);
            GameEvents.OnWaveStarted.RemoveListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Wave_OnWaveEnded);
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates() { }
        
        public override void EnableElement()
        {
            Ctx.LobbyScreenUITransform.gameObject.SetActive(true);
            UpdateUI();
        }

        public override void DisableElement() => Ctx.LobbyScreenUITransform.gameObject.SetActive(false);

        private void UpdateUI()
        {
            Ctx.TotalGearsCount.text = GameManager.Instance.TotalGearAmount.ToString();
            
            Ctx.CurrentWaveCount.text = GameManager.Instance.WaveStateMachine.CurrentWaveID == 0 
                ? "Tutorial" 
                : GameManager.Instance.WaveStateMachine.CurrentWaveID.ToString();
        }

        private void GameEvents_Item_OnTotalGearAmountChanged() => UpdateUI();

        private void GameEvents_Wave_OnWaveEnded() => UpdateUI();

        private void GameEvents_Wave_OnWaveStarted() => SwitchState(Factory.UIGame());
    }
}
